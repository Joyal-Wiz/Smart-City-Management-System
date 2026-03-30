using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCity.Domain.Entities
{
    public class Worker
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsAvailable { get; set; } = true;
        public string Status { get; set; } = "Pending";

    }
}
