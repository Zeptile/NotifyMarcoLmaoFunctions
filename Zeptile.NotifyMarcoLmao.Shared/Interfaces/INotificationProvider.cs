using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zeptile.NotifyMarcoLmao.Shared.Models;

namespace Zeptile.NotifyMarcoLmao.Shared.Interfaces
{
    public interface INotificationProvider
    {
        Task<List<NotificationItem>> GetAllItemsAsync();
        Task<List<NotificationItem>> GetItemWhereDue();
        Task InsertItem(NotificationItem item);
        Task UpdateItem(NotificationItem item);
    }
}