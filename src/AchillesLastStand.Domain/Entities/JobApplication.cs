namespace AchillesLastStand.Domain.Entities
{
    // DOMAIN LAYER - ENTITY
    // This is a Domain Entity that represents the core business concept of a Job Application.
    // Following Clean Architecture, this class has NO dependencies on external frameworks or libraries.
    // This achieves SOLID principles: Single Responsibility (only represents job application data).
    public class JobApplication
    {
        public int Id { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string AppliedFromPlatform { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? Payment { get; set; }
        public string? Contact { get; set; }
        public string? Observation { get; set; }
    }
}