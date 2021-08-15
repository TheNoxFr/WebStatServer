using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceStatServer.Interfaces;
using ServiceStatServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace ServiceStatServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGenesys, Genesys>();
            services.AddSingleton<IDAL, DAL>();
            services.AddSingleton<IDonnees, Donnees>();
            services.AddSingleton(Log.Logger);
            services.AddGrpc();

            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

            /*
            Log.Logger = new LoggerConfiguration().WriteTo.File("c:\\GCTI\\Logs\\ServiceStatServer\\ServiceStatServer.txt", rollOnFileSizeLimit:true, fileSizeLimitBytes: 100000,
                retainedFileCountLimit:5).CreateLogger();
              */  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //loggerFactory.AddFile("c:\\GCTI\\Logs\\ServiceStatServer\\ServiceStatServer.txt");
            //loggerFactory.AddFile("c:\\GCTI\\Logs\\ServiceStatServer\\ServiceStatServer.txt", fileSizeLimitBytes : 1000, retainedFileCountLimit: 5);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<StatService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
