﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using SilentLux.Services;
using SilentLux.Services.Interfaces;
using System.Collections.Generic;

namespace Pluralsight.AspNetCore.Auth.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            var users = new Dictionary<string, string> { { "test", "password" } };
            services.AddSingleton<IUserService>(new DummyUserService(users));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddFacebook(options =>
                {
                    options.AppId = "200069633984414";
                    options.AppSecret = "33d4cba728e720b6afbb6f2751e6be9d";
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/signin";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRewriter(new RewriteOptions().AddRedirectToHttps(301, 44343));

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}