using AuthService.Api.Models;
using AuthService.Api.Payloads.Messages;
using AuthService.Api.Payloads.Requests;
using AuthService.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IServiceBus _serviceBus;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IServiceBus serviceBus)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _serviceBus = serviceBus;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest request)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = request.Username,
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Birthdate = request.Birthdate.Date,
                CreatedOn = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);

                var message = new AccountMessage()
                {
                    UserId = Guid.Parse(user.Id),
                    Username = user.UserName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                };

                await _serviceBus.SendMessageAsync(message, topicName:"user-registration");

                return StatusCode(StatusCodes.Status201Created, "User registered succesfully");
            }

            foreach (var error in result.Errors)
            {   
                ModelState.AddModelError("Errors", error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Username);

            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                    var expiryDate = DateTime.Now.AddDays(2);

                    var token = new JwtSecurityToken(
                        expires: expiryDate,
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        Username = user.UserName,
                        UserId = user.Id,
                        UserRoles = userRoles,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        TokenExpiry = expiryDate
                    });
                }
            }
            return Unauthorized();
        }
    }
}