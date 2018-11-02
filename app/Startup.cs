using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;



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

            services.AddAuthentication(options =>
            {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                ValidateIssuer = false,
                ValidIssuer = Configuration["JWTTokenConfiguration:Issuer"],
                ValidateAudience = false,
                ValidAudience = Configuration["JWTTokenConfiguration:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTTokenConfiguration:Key"])),
            
                };
            });

            services.AddMvc();
            services.AddAutoMapper();

            services.AddScoped<Retrospective.Data.Database, Retrospective.Data.Database>();

            services.AddTransient<Retrospective.Domain.TeamManager, Retrospective.Domain.TeamManager>();
            services.AddTransient<Retrospective.Domain.MeetingManager, Retrospective.Domain.MeetingManager>();

            services.Configure<JWTTokenConfiguration>(Configuration.GetSection("JWTTokenConfiguration"));
            
            //services.UseAngularCliServer();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../client";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

                /* 
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
                */
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
    
            //for JWT
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                
            });

            app.UseSpa( spa =>
            {
                //spa.Options.DefaultPage = "/index.html";
                spa.Options.SourcePath = "../client";

                if(env.IsDevelopment())
                {
                    spa.UseAngularCliServer(  "start");
                }
            });




    
        }
    }
}
