using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using AspNetCore.Identity.MongoDbCore.Models;
using HotelReservationAPI.Data;
using HotelReservationAPI.Extensions;
using HotelReservationAPI.Helper.MailService;
using HotelReservationAPI.Helper.MongoSettings;
using HotelReservationAPI.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationAPI
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
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
                (
                    mongoDbSettings.ConnectionString, mongoDbSettings.Name
                );
            services.AddControllers();
           
            services.AddCors(c =>  
            {  
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());  
            });

            services.ConfigureIdentityPassword();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.ConfigureJwt(Configuration);
            services.ConfigureAppUserService();
            services.ConfigureAuthManager();
            services.ConfigureLoggerService();
            services.ConfigureSwagger();
            services.ConfigureRoomService();
            services.ConfigureRoomRepository();
            services.ConfigureReservationRepository();
            services.ConfigureReservationService();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelReservationAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //PreSeeder.Seed(roleManager, userManager).Wait();

            app.UseCors(options => options.AllowAnyOrigin());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
