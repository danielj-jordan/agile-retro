using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.SpaServices.Webpack;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.AngularCli;
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
      Console.WriteLine("injected Configuration");
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

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
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTTokenConfiguration:Key"])),

            };
            config.RequireHttpsMetadata =  false;
            config.SaveToken = true;
            config.TokenValidationParameters=temp;
          });


      services.AddMvc();
      services.AddAutoMapper();

      services.AddScoped<Retrospective.Data.Database, Retrospective.Data.Database>();

      services.AddTransient<Retrospective.Domain.TeamManager, Retrospective.Domain.TeamManager>();
      services.AddTransient<Retrospective.Domain.MeetingManager, Retrospective.Domain.MeetingManager>();
      services.AddTransient<Retrospective.Domain.CommentManager, Retrospective.Domain.CommentManager>();

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

      //services.UseAngularCliServer();
      /*
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "../web/client";
      });
      */
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

      //app.UseStaticFiles();

      if (env.IsDevelopment())
      {
       // app.UseSpaStaticFiles();
      }

      //for JWT
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
          ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });
      app.UseAuthentication();

      app.UseMvc();

/* 
      app.UseMvc(routes =>
      {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
      });
*/
/* 
      if (env.IsDevelopment())
      {
        app.UseSpa(spa =>
        {
          //spa.Options.DefaultPage = "/index.html";
          spa.Options.SourcePath = "../web/client";

          if (env.IsDevelopment())
          {
           // spa.UseAngularCliServer("start");
          }
        });
      }
      */
    }
  }
}