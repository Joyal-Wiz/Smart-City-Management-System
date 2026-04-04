using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCity.Application.Interfaces
{
    public interface INotificationService
    {
        Task CreateAsync(string title, string message, string type, Guid? relatedId = null);
    }
}
