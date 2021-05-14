using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Zeptile.NotifyMarcoLmao.Shared.Interfaces;
using Zeptile.NotifyMarcoLmao.Shared.Models;

namespace Zeptile.NotifyMarcoLmao.Poll
{
    public class PollTrigger
    {
        private readonly IConfiguration _config;
        private readonly INotificationProvider _notificationProvider;
        private readonly RestClient _httpClient;
        
        public PollTrigger(IConfiguration config, INotificationProvider notificationProvider)
        {
            _config = config;
            _notificationProvider = notificationProvider;
            _httpClient = new RestClient(_config["WebhookFlowEndpoint"]);
            _httpClient.UseNewtonsoftJson();
        }
        
        [FunctionName("PollTrigger")]
        public async Task RunAsync([TimerTrigger("0 0,30 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                var items = await _notificationProvider.GetItemWhereDue();

                foreach (var item in items)
                {
                    log.LogInformation($"Sending Overdue item ID: {item.Id}");
                    
                    var req = new RestRequest().AddJsonBody(new DiscordWebhook
                    {
                        Content = $"<@{_config["DiscordUserId"]}>, " + item.Content
                    });

                    await _httpClient.PostAsync<dynamic>(req);

                    item.Status = NotificationStatus.Completed;
                    await _notificationProvider.UpdateItem(item);
                }
            }
            catch (Exception e)
            {
                log.LogError($"Error while polling for notifications :: {e}");
                throw;
            }
        }
    }
}