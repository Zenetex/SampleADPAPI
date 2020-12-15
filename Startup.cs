using System;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SampleADPAPI.Contracts;
using SampleADPAPI.Services;

[assembly: FunctionsStartup(typeof(SampleADPAPI.Startup))]
namespace SampleADPAPI
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder != null)
            {
                ADPKeyVault secretVault = new ADPKeyVault(
                 new Uri("https://globalsecrets.vault.usgovcloudapi.net/"),
                    "WorkforceNowAPI",
                    "WorkforceNowAPI-ClientID",
                    "WorkforceNowAPI-Secret"
                    );
                WorkforceNowService adpService = new WorkforceNowService(
                    secretVault,
                    new Uri("https://accounts.adp.com/auth/oauth/v2/token?")
                    );

                builder.Services.AddTransient<ADPEventService>(s => (ADPEventService)new EventNotificationService(
                    adpService,
                    new Uri("https://api.adp.com/core/v1/event-notification-messages")
                    ));
            }
        }
    }
}
