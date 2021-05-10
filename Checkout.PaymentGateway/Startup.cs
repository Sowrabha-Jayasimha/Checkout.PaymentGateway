using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using Checkout.PaymentGateway.Data;
using Checkout.PaymentGateway.Manager;
using Checkout.PaymentGateway.Manager.Clients;
using Microsoft.OpenApi.Models;
using AutoMapper;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Checkout.PaymentGateway
{
    public class Startup
    {
        private const string ApiName = "Checkout - Payment Gateway API";
        private const string ApiVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TransactionDbConnection"),
                    b => b.MigrationsAssembly("Checkout.PaymentGateway")));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IPaymentManager, PaymentManager>();

            services.AddHttpClient<IBankClient, BankClient>(http => { http.BaseAddress = new Uri("https://localhost:44323/api/v1/"); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName);
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
