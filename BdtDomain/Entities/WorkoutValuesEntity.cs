using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BdtShared.Entities;

[Table("WorkoutValues")]
public class WorkoutValuesEntity : IEntity<Guid>
{
    [Column("Id")]
    [Key]
    public Guid Id { get; set; }

    [Column("WorkoutId")]
    [Required]
    public Guid WorkoutId { get; set; }

    [Column("WorkoutTypeId")]
    [Required]
    public int WorkoutTypeId { get; set; }

    [Column("Amount")]
    [Required]
    public int Amount { get; set; }

    [ForeignKey(nameof(WorkoutId))]
    [JsonIgnore]
    public WorkoutEntity? Workout { get; set; }

    [ForeignKey(nameof(WorkoutTypeId))]
    public WorkoutTypeEntity? WorkoutType { get; }
}
