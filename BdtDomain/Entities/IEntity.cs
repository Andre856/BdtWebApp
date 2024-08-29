namespace BdtShared.Entities;

public interface IEntity<TId>
{
    TId Id { get; set; }
}
