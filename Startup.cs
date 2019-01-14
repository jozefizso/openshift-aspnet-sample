using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace openshift_aspnet_sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            this.Configuration.GetSection("OPENSHIFT_BUILD");

            app.Run(async (context) =>
            {
                var config = this.Configuration;

                var message = $"Hello from ASP.NET Core app.\n\n" +
                    $"Running on OpenShift server\n\n"+
                    $"Host: {Environment.MachineName}\n" +
                    $"EnvironmentName: {env.EnvironmentName}\n\n" +
                    $"OPENSHIFT_BUILD_COMMIT={config.GetValue<string>("OPENSHIFT_BUILD_COMMIT")}\n" +
                    $"OPENSHIFT_BUILD_NAME={config.GetValue<string>("OPENSHIFT_BUILD_NAME")}\n" +
                    $"OPENSHIFT_BUILD_NAMESPACE={config.GetValue<string>("OPENSHIFT_BUILD_NAMESPACE")}\n" +
                    $"OPENSHIFT_BUILD_REFERENCE={config.GetValue<string>("OPENSHIFT_BUILD_REFERENCE")}\n" +
                    $"OPENSHIFT_BUILD_SOURCE={config.GetValue<string>("OPENSHIFT_BUILD_SOURCE")}\n"; ;

                await context.Response.WriteAsync(message);
            });
        }
    }
}
