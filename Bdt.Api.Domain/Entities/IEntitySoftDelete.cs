namespace Bdt.Api.Domain.Entities;

public interface IEntitySoftDelete
{
    public bool IsDeleted { get; set; }
}
