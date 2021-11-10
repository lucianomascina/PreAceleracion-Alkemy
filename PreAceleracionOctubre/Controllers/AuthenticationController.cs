using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;
using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using PreAceleracionOctubre.ViewModel.Auth.Login;
using PreAceleracionOctubre.ViewModel.Auth.Register;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mailService;

        public AuthenticationController(UserManager<User> usermanager, RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager, IMailService mailService)
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        [HttpPost]
        [Route("registro")]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);

                if (userExists != null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new
                        {
                            Status = "Error",
                            Message = $"User creation failed! Errors: {string.Join(",", result.Errors.Select(x => x.Description))}"
                        });
                }

                await _mailService.SendEmail(user);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(new
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }

        [HttpPost]
        [Route("registro-admin")]
        public async Task<IActionResult> RegistroAdmin(RegisterRequestModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);

                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new
                        {
                            Status = "error",
                            Message = $"User creation failed! The user with username {model.Username} already exists."
                        });
                }

                var user = new User
                {
                    UserName = model.Username,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new
                        {
                            Status = "Error",
                            Message = $"User creation failed! Errors: {string.Join(",", result.Errors.Select(x => x.Description))}"
                        });
                }

                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                await _userManager.AddToRoleAsync(user, "Admin");
                await _mailService.SendEmail(user);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(new
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if(result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.Username);

                if(currentUser.IsActive)
                {
                    return Ok(await GetToken(currentUser));
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized,
                    new
                {
                    Status = "Error",
                    Message = $"El usuario {model.Username} no esta autorizado!"
                });
        }

        private async Task<LoginResponseViewModel> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currentUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAutorizacion"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponseViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };

        }
    }
}
