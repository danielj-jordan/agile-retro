using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using AutoMapper;
using app.Model;
using Retrospective.Data;
using Google.Apis.Auth;

namespace app.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {

    private readonly ILogger<AuthController> logger;
    private readonly IMapper mapper;
    private readonly IDatabase database;
    private readonly IOptions<JWTTokenConfiguration> tokenConfig;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, IDatabase database, IOptions<JWTTokenConfiguration> tokenConfig)
    {
      this.logger = logger;
      this.mapper = mapper;
      this.database = database;
      this.tokenConfig = tokenConfig;
    }


    [Authorize]
    [HttpGet("[action]")]
    public ActionResult IsLoggedIn()
    {
      return Ok();
    }


    [HttpPost("[action]")]
    public ActionResult<UserLoginToken> GenerateToken([FromBody] UserLogin login)
    {

      if (string.IsNullOrEmpty(login.LoginName))
      {
        logger.LogWarning("username not supplied");
        return new BadRequestResult();
      }

      logger.LogDebug("login: " + login.LoginName);

      // authentication successful so generate jwt token
      var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(tokenConfig.Value.Key);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Name, login.LoginName)
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      UserLoginToken userToken = new UserLoginToken();
      userToken.Token = tokenHandler.WriteToken(token);

      return userToken;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<UserLoginToken>> LoginGoogle([FromBody] UserLoginToken googleToken)
    {
      logger.LogInformation("LoginGoogle called with {0}", googleToken.Token);
      string googleAppId = "266959264581-v4il3u57njfbhreg38tj013u9ahbf8t5.apps.googleusercontent.com";
      GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
      settings.Audience = new List<string>{
          googleAppId
      };

      try{
      var validPayload = await GoogleJsonWebSignature.ValidateAsync(googleToken.Token, settings);

      }
      catch (InvalidJwtException ex)
      {
        //token is not valid
      }
      catch(Exception ex)
      {
        //other error

      }

 


      //get name and email address
      var email=validPayload.Prn;
      var name=validPayload.Name;

      //confirm the user is in our db


      
  
      


      var user = database.Users.FindUserByEmail(email);
      if( user==null)
      {
        var newUser =  new User
        {
          Name=name,
          Email=email,
          IsDemoUser=false,
        };
        user= database.Users.SaveUser(newUser);
      }


      //return a new token
      var userToken = this.CreateToken(user.Email);

      return userToken;
    }


    [HttpPost("[action]")]
    public ActionResult<UserLoginToken> Demo()
    {
      string demoUser = "demo@localhost";

      // authentication successful so generate jwt token
      var userToken = this.CreateToken(demoUser);

      return userToken;

    }

    private UserLoginToken CreateToken(string username)
    {
      var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(tokenConfig.Value.Key);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Name, username)
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      UserLoginToken userToken = new UserLoginToken();
      userToken.Token = tokenHandler.WriteToken(token);

      return userToken;
    }
  }
}