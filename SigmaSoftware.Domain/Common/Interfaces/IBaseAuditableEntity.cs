namespace SigmaSoftware.Domain.Common.Interfaces;

public interface IBaseAuditableEntity
{
    Guid? CreatorUserId { get; set; }
    DateTime CreationTime { get; set; }
    Guid? LastModifierUser { get; set; }
    DateTime? LastModificationTime { get; set; }
    bool IsDeleted { get; set; }
    Guid? DeleterUserId { get; set; }
    DateTime? DeletionTime { get; set; }
}