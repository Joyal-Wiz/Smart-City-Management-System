using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCity.Application.Features.Notifications.Commands.MarkAdminNotificationsAsRead
{
    public class MarkAdminNotificationsAsReadCommand : IRequest<bool>
    {
    }
}
