using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OF.API.Base.Authentication;
using OF.API.Base.Authorization;

namespace OF.API.Base.Authentication    
{
    public class ApiKeyMiddleware<U, A> where U : IUserAuthentication where A : IApiKeyAuthentication
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/api")))
            {
                //Let's check if this is an API Call
                if (context.Request.Headers.Keys.Contains("ApiKey", StringComparer.InvariantCultureIgnoreCase))
                {
                    // validate the supplied API key
                    // Validate it
                    var headerKey = context.Request.Headers["ApiKey"].FirstOrDefault();
                    await ValidateApiKey(context, _next, headerKey);
                }
                else if (context.Request.Query.ContainsKey("apikey"))
                {
                    if (context.Request.Query.TryGetValue("apikey", out var queryKey))
                    {
                        await ValidateApiKey(context, _next, queryKey);
                    }
                }
                // User access token (only by header for security)
                else if (context.Request.Headers["UserAccessToken"].Any())
                {
                    var headerKey = context.Request.Headers["UserAccessToken"].FirstOrDefault();
                    await ValidateUserAccessToken(context, _next, headerKey);
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private async Task ValidateUserAccessToken(HttpContext context, RequestDelegate next, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                await context.Response.WriteAsync("Invalid User Access Token");
                return;
            }

            var userService = context.RequestServices.GetRequiredService<IUserServiceBasic<U>>();
            var user = userService.GetUserByAccessToken(key);
            if (user == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid User Access Token");
            }
            else
            {
                var roleService = context.RequestServices.GetRequiredService<IRoleServiceBasic>();
                var identity = new GenericIdentity(user.GetUserName());
                var roles = roleService.GetUserFunctionalities(user.GetUserId());
                var principal = new GenericPrincipal(identity, roles.ToArray());
                context.User = principal;
                await next.Invoke(context);
            }
        }
        
        private async Task ValidateApiKey(HttpContext context, RequestDelegate next, string key)
        {
            var apikeyService = context.RequestServices.GetRequiredService<IApiKeyServiceBasic<A>>();
            var apiKey = apikeyService.GetAuthorizedApiKeys().FirstOrDefault(a => a.GetKey().Equals(key));
            var valid = apiKey != null;
            if (!valid)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
            }
            else
            {
                if (context.Request.Headers.Keys.Contains("UserName", StringComparer.InvariantCultureIgnoreCase) && !apiKey.AllowImpersonation())
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("API Key do not allow impersionation");
                    await next.Invoke(context);
                    return;
                }
                    // Check if we have a UserName header if so we can impersonate that user
                if (apiKey.AllowImpersonation() && context.Request.Headers.Keys.Contains("UserName", StringComparer.InvariantCultureIgnoreCase))
                {
                    var username = context.Request.Headers["UserName"].FirstOrDefault();
                    if (string.IsNullOrEmpty(username))
                    {
                        UseApiUser(context);
                    }
                    var userService = context.RequestServices.GetRequiredService<IUserServiceBasic<U>>();
                    var user = userService.GetUserByUserName(username);
                    if (user == null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("Invalid User");
                        await next.Invoke(context);
                        return;
                    }
                    var roleService = context.RequestServices.GetRequiredService<IRoleServiceBasic>();
                    var roles = roleService.GetUserFunctionalities(user.GetUserId());
                    var identity = new GenericIdentity(user.GetUserName());
                    var principal = new GenericPrincipal(identity, roles.ToArray());
                    context.User = principal;
                }
                else
                {
                    UseApiUser(context);
                }

                await next.Invoke(context);
            }
        }

        private void UseApiUser(HttpContext context)
        {
            var userService = context.RequestServices.GetRequiredService<IUserServiceBasic<U>>();
            var roleService = context.RequestServices.GetRequiredService<IRoleServiceBasic>();
            var user = userService.GetDefaultUserForApiKey();
            var roles = roleService.GetUserFunctionalities(user.GetUserId());
            var identity = new GenericIdentity(user.GetUserName());
            var principal = new GenericPrincipal(identity, roles.ToArray());
            context.User = principal;
        }
    }
}