using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using AutoMapper;
using app.Model;
using Retrospective.Data;



namespace app.Controllers
{
    [Route("api/[controller]")]
    public class AuthController: Controller
    {

        private readonly ILogger<AuthController>  _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        private readonly IOptions<JWTTokenConfiguration> tokenConfig;

        public AuthController(ILogger<AuthController> logger, IMapper mapper,Database database, IOptions<JWTTokenConfiguration> tokenConfig)
        {
            _logger=logger;
            _mapper=mapper;
            this.database=database;
            this.tokenConfig=tokenConfig;
        }


        [HttpPost("[action]")]
        public ActionResult<UserLoginToken> GenerateToken ([FromBody] UserLogin login){
            
            if(string.IsNullOrEmpty(login.LoginName)){
                _logger.LogWarning("username not supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug ("login: " + login.LoginName);

             // authentication successful so generate jwt token
            var tokenHandler = new  System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
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
            userToken.Token= tokenHandler.WriteToken(token);

            return userToken;



        }

        [HttpPost("[action]")]
        public ActionResult<UserLoginToken> Demo (){
            

             // authentication successful so generate jwt token
            var tokenHandler = new  System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenConfig.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, "demo@localhost")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginToken userToken = new UserLoginToken();
            userToken.Token= tokenHandler.WriteToken(token);

            return userToken;



        }    
    }
}