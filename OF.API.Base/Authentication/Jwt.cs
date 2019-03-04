using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace OF.API.Base.Authentication
{
    public static class Jwt
    {
        public enum CustomClaimTypes
        {
            Password,
            IP
        }

        public static void AddJwtAuthentication<U, S>(this IServiceCollection services, string keyText, bool validateIP, bool validatePasswordChanged, int timeoutSeconds) where U : IUserAuthentication where S : ISessionAuthetication
        {
            var key = Encoding.ASCII.GetBytes(keyText);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserServiceBasic<U>>();
                            var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionServiceBasic<S>>();
                            var userId = int.Parse(context.Principal.Identity.Name);
                            var user = userService.GetUserById(userId);
                            Claim tokenPassword = context.Principal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Password.ToString());
                            Claim tokenIP = context.Principal.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.IP.ToString());

                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            if (validateIP && !context.HttpContext.Connection.RemoteIpAddress.ToString().Equals(tokenIP.Value))
                            {
                                context.Fail("Unauthorized. Invalid IP");
                            }
                            if (validatePasswordChanged && user != null && !user.GetUserPasword().Equals(tokenPassword.Value))
                            {
                                context.Fail("Unauthorized. User password has changed");
                            }

                            var activeSession = sessionService.GetValidSessions(userId).FirstOrDefault(s => s.GetToken().Equals(((System.IdentityModel.Tokens.Jwt.JwtSecurityToken)context.SecurityToken).RawData));
                            if (activeSession == null)
                            {
                                context.Fail("Unauthorized. Invalid Token");
                            }
                            else
                            {
                                if (activeSession.GetLastAccess().AddSeconds(timeoutSeconds).CompareTo(DateTime.Now) == -1)
                                {
                                    context.Fail(new SecurityTokenExpiredException());
                                    sessionService.UpdateActive(activeSession.GetId(), false);
                                }
                                else
                                {
                                    sessionService.UpdateAccess(activeSession.GetId(), DateTime.Now);
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
