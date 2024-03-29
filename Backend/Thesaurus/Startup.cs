using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Thesaurus.DAL;
using Thesaurus.Services;

namespace Thesaurus
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
            services.AddMemoryCache();
            services.AddCors(options =>
            options.AddPolicy("DefaultCorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod())
                //.AllowCredentials())
        );
            services.AddInfrastructure();        
            services.AddBusinessServices();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\Dapper.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Thesaurus", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log_{long.Parse(DateTime.Now.ToString("yyyyMMdd"))}.txt");

            

            //app.UseExceptionHandler(c => c.Run(async context =>
            //{
            //    var exception = context.Features
            //        .Get<IExceptionHandlerPathFeature>()
            //        .Error;
            //    var response = new { error = exception.Message };
            //    await context.Response.WriteAsJsonAsync(response);
            //}));

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thesaurus v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("DefaultCorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
