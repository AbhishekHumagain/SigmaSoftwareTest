using System.ComponentModel.DataAnnotations;
using SigmaSoftware.Domain.Common.Interfaces;

namespace SigmaSoftware.Domain.Entities;

public class Candidate : IBaseAuditableEntity
{
    [Key] [MaxLength(50)] [EmailAddress] public required string Email { get; set; } // Unique identifier

    [MaxLength(50)] public required string FirstName { get; set; }

    [MaxLength(50)] public required string LastName { get; set; }

    [Phone] [MaxLength(10)] public string? PhoneNumber { get; set; }

    [MaxLength(10)] public string? CallTimeInterval { get; set; }

    [Url] [MaxLength(20)] public string? LinkedInProfileUrl { get; set; }

    [Url] [MaxLength(20)] public string? GitHubProfileUrl { get; set; }

    [MaxLength(200)] public required string Comment { get; set; } // Free text comment
    public Guid? CreatorUserId { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? LastModifierUser { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeleterUserId { get; set; }
    public DateTime? DeletionTime { get; set; }
}