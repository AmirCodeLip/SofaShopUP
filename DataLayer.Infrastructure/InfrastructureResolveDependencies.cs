﻿using AutoMapper;
using DataLayer.Access;
using DataLayer.Access.Data;
using DataLayer.Domin.Models.Identity;
using DataLayer.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataLayer.Infrastructure
{
    public static class InfrastructureResolveDependencies
    {
        private static void addInternals(this IServiceCollection services)
        {
            var type = typeof(InfrastructureResolveDependencies);
            var types = type.Assembly.GetTypes().Where(x => x.Namespace != null && x.Namespace.StartsWith("DataLayer.Infrastructure.Infrastructure"));
            foreach (var Infrastructure in types.Where(n => n.Name.EndsWith("Structure")))
            {
                services.AddScoped(Infrastructure);
            }
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));
            services.AddAccessServices();
            services.addInternals();
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper());
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("shopdb"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
            services.AddIdentity<WebUser, WebRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
            AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:JWT"].ToString())),
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // ValidAudience = "http://dotnetdetail.net",
                    // ValidIssuer = "http://dotnetdetail.net",
                };
            });
            return services;
        }
    }
}
