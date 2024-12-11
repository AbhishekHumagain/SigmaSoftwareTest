using System.ComponentModel.DataAnnotations;

namespace SigmaSoftware.Application.Candidate.Dto
{
    public class CandidateRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? CallTimeInterval { get; set; } 

        public string? LinkedInUrl { get; set; } 

        public string? GitHubUrl { get; set; } 

        public required string FreeTextComment { get; set; } 
    }

}
