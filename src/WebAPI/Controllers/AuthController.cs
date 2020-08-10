using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Authorize, ApiController, ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Success!");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _authService.Authenticate(model, HttpContext.GetIPAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "x-RefreshToken")] string refreshToken)
        {
            var response = await _authService.RefreshToken(refreshToken, HttpContext.GetIPAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token supplied" });

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            var token = model.Token;
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is not supplied" });

            var response = await _authService.RevokeToken(token, HttpContext.GetIPAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }
    }
}
