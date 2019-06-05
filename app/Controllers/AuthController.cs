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
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Domain;
using Google.Apis.Auth;

namespace app.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {

    private readonly ILogger<AuthController> logger;
    private readonly IMapper mapper;
    private readonly UserManager usermanager;
    private readonly IOptions<JWTTokenConfiguration> tokenConfig;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager usermanager, IOptions<JWTTokenConfiguration> tokenConfig)
    {
      this.logger = logger;
      this.mapper = mapper;
      this.usermanager = usermanager;
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

      try
      {
        DomainModel.User savedUser;
        var validPayload = await GoogleJsonWebSignature.ValidateAsync(googleToken.Token, settings);

        Console.WriteLine(validPayload);

        logger.LogInformation("logging in {0} {1}.", validPayload.Name, validPayload.Email);

        //confirm the user is in our db
        var user = usermanager.GetUserFromEmail(validPayload.Email);
        if (user == null)
        {
          logger.LogDebug("creating new user");
          var newUser = new DomainModel.User
          {
            Name = validPayload.Name,
            Email = validPayload.Email,
            IsDemoUser = false,
            AuthenticationSource="Google",
            LastLoggedIn = DateTime.UtcNow,
            AuthenticationID=validPayload.Subject
          };

          savedUser = usermanager.UpdateUser(newUser);
          logger.LogInformation("Created new user with email: {0} and userid: {1} ", savedUser.Email, savedUser.UserId);
        }
        else
        {
          user.LastLoggedIn=DateTime.UtcNow;
          savedUser = usermanager.UpdateUser(user);
          logger.LogInformation("updating user with email: {0} and userid: {1} ", savedUser.Email, savedUser.UserId);
        }

        //return a new token
        var userToken = this.CreateToken(savedUser.Email, savedUser.UserId);
        return userToken;
      }
      catch (InvalidJwtException ex)
      {
        logger.LogWarning("The token is not valid {0}", ex.Message );
      }
      catch (Exception ex)
      {
        logger.LogWarning("Could not authenciate using Google Token {0} {1}", googleToken, ex.Message );
      }

      return null;
    }


    [HttpPost("[action]")]
    public ActionResult<UserLoginToken> Demo()
    {
      string demoUser = "demo@localhost";
      var user = usermanager.GetUserFromEmail(demoUser);

      // authentication successful so generate jwt token
      var userToken = this.CreateToken(user.Email, user.UserId);

      return userToken;

    }

    private UserLoginToken CreateToken(string username, string userId)
    {
      var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(tokenConfig.Value.Key);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Email, username),
                    new Claim(ClaimTypes.PrimarySid, userId)
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