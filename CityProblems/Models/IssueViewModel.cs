namespace CityProblems.Models
{
    public class IssueViewModel
    { 
        public string Category { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Latitude { get; set; } = null!;

        public string Longitude { get; set; } = null!;

        public string Photo { get; set; } = null!;
    }
}
