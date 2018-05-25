﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Birder2.Data;
using Birder2.Models;
using Birder2.Services;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Birder2
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
            //services.Configure<StorageAccountOptions>(Configuration.GetSection("BlobService"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies 
                // is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
                //.AddCookie(options =>
                //{
                //    ...
                //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                // Password settings: require any eight letters or numbers
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper();

            // Add application service s.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IMachineClockDateTime, MachineClockDateTime>();

            services.AddTransient<IApplicationUserAccessor, ApplicationUserAccessor>();

            services.AddScoped<IBirdRepository, BirdRepository>();
            services.AddScoped<IObservationRepository, ObservationRepository>();
            services.AddScoped<ISideBarRepository, SideBarRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //ToDo: Work out what type of service these should be - Singletons?
            services.AddTransient<IStream, StreamService>();
            services.AddTransient<IFlickrService, FlickrService>();

            services.AddTransient<IImageStorageService, ImageStorageService>();

            services.AddMvc().AddJsonOptions
                (options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (env.IsProduction())
            {
                var options = new RewriteOptions()
                    .AddRedirectToHttps();

                app.UseRewriter(options);
            }

            //app.UseWelcomePage()

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Observation}/{action=Index}/{id?}");
            });
        }
    }
}
