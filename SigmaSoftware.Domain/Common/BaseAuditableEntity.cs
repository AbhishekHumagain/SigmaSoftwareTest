using SigmaSoftware.Domain.Common.Interfaces;

namespace SigmaSoftware.Domain.Common;

public abstract class BaseAuditableEntity : Events, IBaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? CreatorUserId { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? LastModifierUser { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeleterUserId { get; set; }
    public DateTime? DeletionTime { get; set; }
}
