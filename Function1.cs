using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using SampleADPAPI.Contracts;
using SampleADPAPI.Services;
using SampleADPAPI.Models.EventNotification;

namespace SampleADPAPI
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            log.LogInformation("Starting Test");
            log.LogInformation("Creating Key Vault");
            ADPKeyVault secretVault = new ADPKeyVault(
                 new Uri(config["KeyVaultURI"]),
                    config["CertificateName"],
                    config["ClientIDName"],
                    config["ClientSecretName"]
                    );
            log.LogInformation("Creating ADP Service");
            WorkforceNowService adpService = new WorkforceNowService(
                secretVault,
                new Uri(config["TokenURI"])
                );
            log.LogInformation("Creating ADP Event Service");
            EventNotificationService eventService = new EventNotificationService(
                adpService,
                new Uri(config["EventNotificationURI"])
                );
            log.LogInformation("Calling Events");
            ADPEventMessage eventMessage = eventService.getEvents();
            foreach (ADPEvent curEvent in eventMessage.Events)
                log.LogInformation(curEvent.EventID + " - " + curEvent.EvenNameCode.CodeValue);
            log.LogInformation("Nothing Crashed");

            return new OkObjectResult(JsonConvert.SerializeObject(eventMessage));
        }
    }
}
