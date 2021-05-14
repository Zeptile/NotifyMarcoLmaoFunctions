using System;
using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace Zeptile.NotifyMarcoLmao.Shared.Models
{
    public class NotificationItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Content { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime DueDate { get; set; }
    }

    public enum NotificationStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}