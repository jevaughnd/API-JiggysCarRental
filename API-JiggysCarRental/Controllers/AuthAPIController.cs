using API_JiggysCarRental.MODELS;
using API_JiggysCarRental.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_JiggysCarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthAPIController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _authService.RegisterUser(user))
            {
                return Ok(new { status = "succes", message = "registration succesfull" });
            }
            return BadRequest(new { status = "fail", message = "registration failed" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _authService.Login(user);
            if (result == true)
            {
                var token = _authService.GenerateToken(user);
                return Ok(new { status = "succes", message = "login succesfull", data = token });
            }
            return BadRequest(new { status = "failed", message = "login failed" });
        }
    }
}
