using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BdtShared.Entities;

[Table("WorkoutTypes")]
public class WorkoutTypeEntity : IEntity<int>
{
    [Column("Id")]
    [Key]
    public int Id { get; set; }

    [Column("Name")]
    [Required]
    public string Name { get; set; }

    [Column("PushUps")]
    public int? PushUps { get; set; }
}
