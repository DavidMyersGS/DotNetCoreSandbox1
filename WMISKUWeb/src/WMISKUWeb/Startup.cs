using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using log4net.Config;
using GameStop.SupplyChain.Logging.log4net;

namespace GameStop.SupplyChain.Services.WMISKUWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            log4net.GlobalContext.Properties["appRoot"] = Configuration["AppSettings:LogFilePath"].ToString();
            XmlConfigurator.Configure(new FileInfo(Path.Combine(env.ContentRootPath, Configuration["AppSettings:LogSettingsFile"].ToString())));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddTransient<ISKUUpsertMessageContract, SKUUpsertMessageContract>();
            services.AddLogging();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddProvider(new Log4NetProvider());
            
            app.UseMvc();
        }
    }
}
