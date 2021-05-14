using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zeptile.NotifyMarcoLmao.Shared.Interfaces;
using Zeptile.NotifyMarcoLmao.Shared.Services;

[assembly: FunctionsStartup(typeof(Zeptile.NotifyMarcoLmao.Post.Startup))]
namespace Zeptile.NotifyMarcoLmao.Post
{
    public class Startup : FunctionsStartup
    {
        
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var cs = Environment.GetEnvironmentVariable("AZ_APPCONFIG_URI");
            builder.ConfigurationBuilder.AddAzureAppConfiguration(cs);
        }
        
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<INotificationProvider, NotificationProvider>();
        }
    }
}