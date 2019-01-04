using AspNetCoreVueJs.Web.Data;
using AspNetCoreVueJs.Web.Data.Entities;
using AspNetCoreVueJs.Web.Infrastructure;
using AspNetCoreVueJs.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreVueJs.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqliteConnectionString = Configuration.GetConnectionString("SqliteConnectionString");
            var dbPath = sqliteConnectionString.Replace("Data Source=", "").Trim();
            if (!System.IO.Path.IsPathFullyQualified(dbPath))
            {
                var contentPath = HostingEnvironment.ContentRootPath;
                dbPath = System.IO.Path.Combine(contentPath, dbPath);
                sqliteConnectionString = "Data Source=" + dbPath;
            }
            services.AddDbContext<EcommerceContext>(options =>
            {
                // options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlite(sqliteConnectionString);
               // options.EnableSensitiveDataLogging(true);
                //options.UseInMemoryDatabase("VueEcommerce");
            });

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<EcommerceContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.Cookie.Name = "YourAppCookieName";
                //options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                //  options.SlidingExpiration = true;
            });


            services.AddAuthentication()
            .AddCookie(options => {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            } )
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.ClaimsIssuer = Configuration["Authentication:JwtIssuer"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Authentication:JwtIssuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["Authentication:JwtAudience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Configuration["Authentication:JwtKey"])),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";

            });

            DbContextExtensions.UserManager =
               services.BuildServiceProvider()
               .GetService<UserManager<AppUser>>();

            var provider = services.BuildServiceProvider();
            DbContextExtensions.UserManager = provider.GetService<UserManager<AppUser>>();
            DbContextExtensions.RoleManager = provider.GetService<RoleManager<AppRole>>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(
                    new FeatureLocationExpander());
            });
            services.Configure<List<SeedUser>>(Configuration.GetSection("SeedUsers"));
            Serilog.Debugging.SelfLog.Enable(Console.Error);
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            StaticFileOptions options = new StaticFileOptions();
            options.RequestPath = "/clientapp";
            app.UseSpaStaticFiles(options);
            app.UseCookiePolicy();

         
            app.UseMvc(routes =>
            {
            //    routes.MapRoute("root", "/",
            //defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute("spa-fallback",
                    new { controller = "Home", action = "Index" });

            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }
            });

            StripeConfiguration.SetApiKey(Configuration["Stripe:PrivateKey"]);
        }
    }
}
