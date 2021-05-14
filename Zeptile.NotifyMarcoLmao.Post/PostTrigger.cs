using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zeptile.NotifyMarcoLmao.Shared.Interfaces;
using Zeptile.NotifyMarcoLmao.Shared.Models;

namespace Zeptile.NotifyMarcoLmao.Post
{
    public class PostTrigger
    {
        private readonly IConfiguration _config;
        private readonly INotificationProvider _notificationProvider;

        public PostTrigger(IConfiguration config, INotificationProvider notificationProvider)
        {
            _config = config;
            _notificationProvider = notificationProvider;
        }
        
        [FunctionName("PostTrigger")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function,  "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<NotificationItem>(requestBody);

                if (data != null)
                {
                    data.Status = NotificationStatus.Pending;
                    await _notificationProvider.InsertItem(data);
                }

                return new OkObjectResult("OK");
            }
            catch (Exception e)
            {
                log.LogError($"Error while trying to add new notification :: {e}");
                return new BadRequestResult();
            }
        }
    }
}