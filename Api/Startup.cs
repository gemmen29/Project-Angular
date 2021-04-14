using BL.Bases;
using BL.Interfaces;
using BL.AppServices;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DAL.ApplicationUserIdentity;

namespace Api
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
          
            services.AddControllers();
            services.AddDbContext<ApplicationDBContext>(option => {
                option.UseSqlServer(Configuration.GetConnectionString("CS"), 
                    options => options.EnableRetryOnFailure());
            });
            //services.AddTransient <,>
            services.AddIdentity<ApplicationUserIdentity, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient< UserManager<ApplicationUserIdentity>>();
            services.AddTransient< RoleManager<IdentityRole>>();
            services.AddTransient<AccountAppService>();
            services.AddTransient<CategoryAppService>();
            services.AddTransient<CartAppService>();
            services.AddTransient<ProductAppService>();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseSwagger();
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
