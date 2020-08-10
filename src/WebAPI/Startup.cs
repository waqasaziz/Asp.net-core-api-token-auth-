using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using System.Text;

using Domain.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;

using Domain.Entities;
using System.Security.Policy;
using WebAPI.Helpers;
using WebApi.Helpers;
using Microsoft.OpenApi.Models;

namespace WebAPI
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
            services.AddDbContext<Database>(opt =>
            {
                opt.UseInMemoryDatabase("PaymentGateWay");
            });

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddAppServices(Configuration);

            services.AddJWTAuthentication(Configuration);

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });

            services.AddSwagger();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Database context, IHashingProvider hashingProvider)
        {
            if (env.IsDevelopment())
            {
                SeedData(app, context, hashingProvider);
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway V1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SeedData(IApplicationBuilder app, Database context, IHashingProvider hashingProvider)
        {
            var salt = hashingProvider.GenerateSalt();
            var pass = hashingProvider.GenerateHash("Test", salt);


            context.Merchants.Add(new Merchant
            {
                Id = 3,
                Name = "Checkout",
                Username = "Test",
                PasswordSalt = salt,
                PasswordHash = pass,


            });
            context.SaveChanges();
        }
    }
}
