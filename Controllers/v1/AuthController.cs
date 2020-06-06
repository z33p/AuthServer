using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using AuthServer.Services;
using AuthServer.Repositories;

namespace AuthServer.Controllers
{
  [Route("v1/accounts")]
  public class AuthController : Controller
  {
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public ActionResult<dynamic> Login([FromBody] User model)
    {
      var user = UserRepository.Get(model.Username, model.Password);

      if (user == null)
        return NotFound(new { message = "Usuário ou senha inválidos" });

      var token = TokenService.GenerateToken(user);
      user.Password = "";
      return new
      {
        user = user,
        token = token
      };
    }

    [HttpGet]
    [Route("anonymous")]
    [AllowAnonymous]
    public string Anonymous() => "Anonymous";

    [HttpGet]
    [Route("employee")]
    [Authorize(Roles = "employee,manager")]
    public string Employee() => "Employee";

    [HttpGet]
    [Route("manager")]
    [Authorize(Roles = "manager")]
    public string Manager() => "Manager";

  }
}