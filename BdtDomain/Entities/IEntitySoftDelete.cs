namespace BdtShared.Entities;

public interface IEntitySoftDelete
{
    public bool IsDeleted { get; set; }
}
