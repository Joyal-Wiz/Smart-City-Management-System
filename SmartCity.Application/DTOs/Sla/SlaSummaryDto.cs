namespace SmartCity.Application.DTOs.Sla
{
    public class SlaSummaryDto
    {
        public int Total { get; set; }
        public int OnTime { get; set; }
        public int Overdue { get; set; }
        public int Critical { get; set; }
    }
}