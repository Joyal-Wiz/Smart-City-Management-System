public class AdminIssueDetailsDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }

    public string? BeforeImagePath { get; set; }   // ImagePath
    public string? AfterImagePath { get; set; }    // ResolvedImagePath

    public string? AssignedWorkerName { get; set; }

    public decimal Salary { get; set; }
    public DateTime? Deadline { get; set; }

    public string? RejectionReason { get; set; }
}