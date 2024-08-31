namespace BdtApi.Domain.Entities;

public interface IEntitySoftDelete
{
    public bool IsDeleted { get; set; }
}
