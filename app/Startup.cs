using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

namespace app
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      string jwtKey = Configuration["JWTTokenConfiguration:Key"];
      if(string.IsNullOrWhiteSpace(jwtKey))
      {
        Console.WriteLine("*** JWT Key is not set ***");

      }



      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
          .AddJwtBearer(config =>
          {
            var key =Configuration["JWTTokenConfiguration:Key"];
            var temp = new TokenValidationParameters
            {
              ValidateIssuer = false,
             // ValidIssuer = Configuration["JWTTokenConfiguration:Issuer"],
              ValidateAudience = false,
             // ValidAudience = Configuration["JWTTokenConfiguration:Audience"],
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            };
            config.RequireHttpsMetadata =  false;
            config.SaveToken = true;
            config.TokenValidationParameters=temp;
          });


      services.AddMvc();
      services.AddAutoMapper();

      services.AddScoped<Retrospective.Data.IDatabase, Retrospective.Data.Database>();

      services.AddTransient<Retrospective.Domain.ITeamManager, Retrospective.Domain.TeamManager>();
      services.AddTransient<Retrospective.Domain.IMeetingManager, Retrospective.Domain.MeetingManager>();
      services.AddTransient<Retrospective.Domain.ICommentManager, Retrospective.Domain.CommentManager>();
      services.AddTransient<Retrospective.Domain.IUserManager, Retrospective.Domain.UserManager>();

      services.Configure<JWTTokenConfiguration>(Configuration.GetSection("JWTTokenConfiguration"));

      services.Configure<ForwardedHeadersOptions>(options =>
      {
          options.ForwardLimit = 2;
         // options.ForwardedForHeaderName = "X-Forwarded-For-My-Custom-Header-Name";

           /* var ipAddressString = Environment.GetEnvironmentVariable("RESTSERVICE_FORWARD_FROM_PROXY_IP");
            if(string.IsNullOrWhiteSpace(ipAddressString))
            {
              ipAddressString="127.0.0.1" ;
            }
            
            foreach(var address in ipAddressString.Split(" "))
            {
                options.KnownProxies.Add(IPAddress.Parse(address));
            }*/

            //forward everything unless specific network provided
            //options.KnownNetworks.Add( new IPNetwork(IPAddress.Parse("10.0.0.0"),8));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
      }
      else
      {
      //  app.UseExceptionHandler("/Home/Error");
      }

      //for JWT
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
          ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });
      app.UseAuthentication();

      app.UseMvc();
    }
  }
}