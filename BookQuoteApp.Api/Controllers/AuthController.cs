using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using BookQuoteApp.Api.Models;
using BookQuoteApp.Api.DTOs;
using BookQuoteApp.Api.Services;

namespace BookQuoteApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO rDto)
        {
            var user = new ApplicationUser
            {
                UserName = rDto.Email,
                Email = rDto.Email,
                Name = rDto.Name,
            };
            var result = await _userManager.CreateAsync(user, rDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(new { message = "User registered successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO lDto)
        {
            var user = await _userManager.FindByEmailAsync(lDto.Email);
            if (user == null)
                return Unauthorized(new {message = ("Invalid email or password")});
            var passwordValid = await _userManager.CheckPasswordAsync(user, lDto.Password);
            if (!passwordValid)
                return Unauthorized(new { message = ("Invalid email or password") });
            var token = _tokenService.GenerateToken(user);
            return Ok(new { token = token });
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            var name = User.FindFirst("name")?.Value;

            return Ok(new { userId, email, name });
        }
    }
}
