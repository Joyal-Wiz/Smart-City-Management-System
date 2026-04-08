using System;

namespace SmartCity.Application.DTOs
{
    public class IssueMapDto
    {
        public Guid Id { get; set; }   

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }
    }
}