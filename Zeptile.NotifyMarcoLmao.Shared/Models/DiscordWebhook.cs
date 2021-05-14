using System;
using System.Collections.Generic;

namespace Zeptile.NotifyMarcoLmao.Shared.Models
{
    public class DiscordWebhook
    {
        public string Content { get; set; }
        public string Avatar_URL { get; set; }
        public List<object> Embeds { get; set; }
        public string Username { get; set; }
    }
}