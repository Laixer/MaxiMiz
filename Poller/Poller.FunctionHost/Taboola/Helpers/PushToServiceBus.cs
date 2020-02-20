using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Poller.Poller;
using Poller.Helper;
using Maximiz.Model;
using Maximiz.Model.Entity;

namespace AzureFunctions
{

    /// <summary>
    /// Used to validate service bus object conversion.
    /// </summary>
    public class PushToServiceBus
    {

        /// <summary>
        /// Constructor to enable DI for other objects.
        /// </summary>
        public PushToServiceBus()
        {
            //
        }

        /// <summary>
        /// Send a premade crud context through the service bus. Thsi is triggered
        /// by a http post method.
        /// </summary>
        /// <param name="req">The incoming http request</param>
        /// <param name="log">The injected logger</param>
        /// <returns>Html ok message</returns>
        [FunctionName("PushToServiceBus")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("Pushing premade message to service bus");

            // Create and serialize object
            var context = new CreateOrUpdateObjectsContext(0, null)
            {
                Action = CrudAction.Update,
                Entity = new Entity[] { new Account() { Name = "myname" }, new Campaign() }
            };
            var json = Json.Serialize(context);

            // Send to service bus
            var con = Environment.GetEnvironmentVariable("MaximizServiceBusSend");
            var nam = Environment.GetEnvironmentVariable("ServiceBusQueueName");
            var queueClient = new QueueClient(con, nam);
            await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(json)));

            // Give user feedback
            return new OkObjectResult($"Service bus was notified with the " +
                $"following object: \n{json}");
        }
    }
}