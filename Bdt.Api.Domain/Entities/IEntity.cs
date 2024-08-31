namespace Bdt.Api.Domain.Entities;

public interface IEntity<TId>
{
    TId Id { get; set; }
}
