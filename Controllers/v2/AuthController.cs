using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Contracts.V2;
using AuthServer.Contracts.V2.Requests;
using AuthServer.Contracts.V2.Responses;
using AuthServer.Services;


namespace AuthServer.Controllers.V2
{
  [ApiController]
  public class AuthController : Controller
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new AuthFailedResponse
        {
          Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        });
      }

      var authResponse = await _authService.RegisterAsync(request.Email, request.Password);

      if (!authResponse.Success)
      {
        return BadRequest(new AuthFailedResponse
        {
          Errors = authResponse.Errors
        });
      }

      return Ok(new AuthSuccessResponse
      {
        Token = authResponse.Token,
        RefreshToken = authResponse.RefreshToken
      });
    }

    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
      var authResponse = await _authService.LoginAsync(request.Email, request.Password);

      if (!authResponse.Success)
      {
        return BadRequest(new AuthFailedResponse
        {
          Errors = authResponse.Errors
        });
      }

      return Ok(new AuthSuccessResponse
      {
        Token = authResponse.Token,
        RefreshToken = authResponse.RefreshToken
      });
    }

    [HttpPost(ApiRoutes.Auth.Refresh)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
      var authResponse = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);

      if (!authResponse.Success)
      {
        return BadRequest(new AuthFailedResponse
        {
          Errors = authResponse.Errors
        });
      }

      return Ok(new AuthSuccessResponse
      {
        Token = authResponse.Token,
        RefreshToken = authResponse.RefreshToken
      });
    }
  }
}