using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using AuthServer.Data;
using AuthServer.Contracts.V2;

namespace AuthServer.Controllers.V2
{
  [Authorize]
  [ApiController]
  public class UsersController : Controller
  {
    private readonly DataContext _dataContext;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public UsersController(DataContext dataContext, TokenValidationParameters tokenValidationParameters)
    {
      _dataContext = dataContext;
      _tokenValidationParameters = tokenValidationParameters;
    }
    [HttpPost(ApiRoutes.Auth.User)]
    public new async Task<ActionResult> User([FromHeader] string Authorization)
    {
      var token = Authorization.Split("Bearer ")[1].ToString();

      var tokenPrincipal = GetPrincipalFromToken(token);

      var user_id = tokenPrincipal.Claims.Single(x => x.Type == "id").Value;

      var user = await _dataContext.Users.FindAsync(user_id);

      if (user == null) return BadRequest("user not found");

      return Ok(user);
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      try
      {
        var tokenValidationParameters = _tokenValidationParameters.Clone();
        tokenValidationParameters.ValidateLifetime = false;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
        if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
        {
          return null;
        }

        return principal;
      }
      catch
      {
        return null;
      }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
      return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
             jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                 StringComparison.InvariantCultureIgnoreCase);
    }
  }
}