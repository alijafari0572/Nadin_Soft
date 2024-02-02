using Application;
using Application.Contracts.Infrastructure;
using Application.DTOs.Account;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Reflection;

namespace Api.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private ITokenService _tokenService;
        private readonly DataBaseContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
             ITokenService tokenService,
            DataBaseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { res = 0, msg = "Error", err = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { res = 1, msg = "Account Created", err = "" });
            }
            else
            {
                return Ok(new { res = 1, msg = "Account Doesn't Create", err = "" });
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { res = 0, msg = "Error", err = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Ok(new { res = 0, msg = "Error", err = "User Not Found." });
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var generatedToken = _tokenService.BuildToken(user, model.RememberMe);
                if (generatedToken != null)
                {
                    return Ok(new { res = 1, msg = "Login", JwtToken = generatedToken });
                }
                else
                {
                    return BadRequest("Something wrong");
                }
            }
            else
            {
                return BadRequest("Something wrong");
            }

        }


    }
}
