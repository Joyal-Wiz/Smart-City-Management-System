using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCity.Domain.Enums
{
    public enum IssueStatus
    {
        Reported = 1,
        Assigned,
        InProgress,
        Resolved,
        Closed,
        Rejected,
    }
}
