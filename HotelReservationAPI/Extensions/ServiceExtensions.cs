using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HotelReservationAPI.Data.Implementation;
using HotelReservationAPI.Data.Interfaces;
using HotelReservationAPI.Services.AuthManger;
using HotelReservationAPI.Services.Implementation;
using HotelReservationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace HotelReservationAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentityPassword(this IServiceCollection services) =>
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                // options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                
            });
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelReservationAPI", Version = "v1" });
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()

                    }

                });
            });

        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt =>
                {
          
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }



        public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILoggerService, LoggerService>();
        public static void ConfigureRoomService(this IServiceCollection services) => services.AddScoped<IRoomService, RoomService>();
        public static void ConfigureRoomRepository(this IServiceCollection services) => services.AddScoped<IRoomRepository, RoomRepository>();
        public static void ConfigureReservationService(this IServiceCollection services) => services.AddScoped<IReservationService, ReservationService>();
        public static void ConfigureReservationRepository(this IServiceCollection services) => services.AddScoped<IReservationRepository, ReservationRepository>();
        public static void ConfigureAppUserService(this IServiceCollection services) => services.AddScoped<IAppUserService, AppUserService>();
        public static void ConfigureAuthManager(this IServiceCollection services) => services.AddScoped<IAuthenticationManager, AuthenticationManager>();

    }
}
