using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Auth.Commands.Login;
using SmartCity.Application.Features.Auth.Commands.Logout;
using SmartCity.Application.Features.Auth.Commands.RefreshToken;
using SmartCity.Application.Features.Auth.Commands.Register;

namespace SmartCity.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse(
                "User registered successfully",
                result
            ));
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse(
                "Login successful",
                result
            ));
        }

        //  REFRESH TOKEN
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<object>.SuccessResponse(
                "Token refreshed successfully",
                result
            ));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest(ApiResponse<object>.FailResponse("Invalid refresh token"));
            }

            return Ok(ApiResponse<object>.SuccessResponse("Logged out successfully", null));
        }
    }
}