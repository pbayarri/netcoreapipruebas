using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using API.Helpers;
using API.Entities;
using AutoMapper;
using API.Services;
using OF.API.Base.Authorization;
using OF.API.Base.Exception;
using OF.API.Base.Utils;
using static OF.API.Base.Authentication.Jwt;
using System.Linq;
using OF.API.Front.Helpers;
using OF.API.Front;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private ISessionService _sessionService;
        private IHateoasHelper _hateoasHelper;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            ISessionService sessionService,
            IRoleService roleService,
            IHateoasHelper hateoasHelper)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _sessionService = sessionService;
            _roleService = roleService;
            _hateoasHelper = hateoasHelper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var requestIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
            claims.AddRange(_roleService.GetUserFunctionalities(user.Id).ToList().Select(f => new Claim(ClaimTypes.Role, f)));
            claims.Add(new Claim(CustomClaimTypes.Password.ToString(), Encoding.UTF8.GetString(user.PasswordHash)));
            claims.Add(new Claim(CustomClaimTypes.IP.ToString(), requestIp));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddSeconds(double.Parse(_appSettings.SessionTimeoutInSeconds, System.Globalization.NumberStyles.Integer)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Session newSession = new Session();
            newSession.CreatedAt = DateTime.Now;
            newSession.Ip = requestIp;
            newSession.IsActive = true;
            newSession.LastAccess = DateTime.Now;
            newSession.Token = tokenString;
            newSession.UserId = user.Id;
            _sessionService.CreateSession(newSession, Util.IsTrue(_appSettings.AuthAllowMultipleSessionsPerUser));

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [FunctionalityRoleAuthorized("UserCreate")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [FunctionalityRoleAuthorized("UsersGet")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            var usersList = new UsersListDto() { Users = userDtos };
            _hateoasHelper.CompleteLinks(usersList, User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList());
            return Ok(usersList);
        }

        [HttpGet("{id}")]
        [FunctionalityRoleAuthorized("UserGet")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetUserById(id);
            var userDto = _mapper.Map<UserDto>(user);
            _hateoasHelper.CompleteLinks(userDto, User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList());

            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [FunctionalityRoleAuthorized("UserUpdate")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                // save 
                _userService.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [FunctionalityRoleAuthorized("UserDelete")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}