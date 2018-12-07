using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNet.Security.OpenIdConnect.Primitives;
using System.IdentityModel.Tokens.Jwt;
using breezenetcore21.Contexts;
using breezenetcore21.Models;
using Microsoft.AspNetCore.Identity;
using breezenetcore21.Repositories.Interfaces;
using breezenetcore21.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MITSDataLib.Seeds;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace breezenetcore21
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                }); ;

            //Setup up database and use OpenIddict
            services.AddDbContext<MITSContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DevConnectionString"));
                options.UseOpenIddict();

            });

            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MITSContext>();

            services.AddAuthorization(options => {
                options.AddPolicy("Faculty", policy => policy.RequireClaim("Role", "Faculty"));
                options.AddPolicy("Student", policy => policy.RequireClaim("Role", "Student"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));

            });

            services.AddOpenIddict()

               .AddCore(options =>
               {
                   options.UseEntityFrameworkCore().UseDbContext<MITSContext>();
               })
               
               .AddServer(options =>
               {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();
                    // Enable the token endpoint.
                    options.EnableTokenEndpoint("/connect/token");

                    // Enable the password flow.
                    options.AllowPasswordFlow();

                    // Accept anonymous clients (i.e clients that don't send a client_id).
                    options.AcceptAnonymousClients();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();

                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    //
                    options.UseJsonWebTokens();
                    //Update for production...
                    options.AddEphemeralSigningKey();

               });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme

            )
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:52980/";
                    options.Audience = "mits_server";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = OpenIdConnectConstants.Claims.Subject,
                        RoleClaimType = OpenIdConnectConstants.Claims.Role
                    };
                });


            services.AddTransient<MITSSeeder>();
            services.AddTransient<IAdminRepository, AdminRepository>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
