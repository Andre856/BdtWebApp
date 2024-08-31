using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdtApi.Domain.Entities;

[Table("Workout")]
public class WorkoutEntity : IEntity<Guid>, IUserEntity
{
    [Column("Id")]
    [Key]
    public Guid Id { get; set; }

    [Column("UserId")]
    [Required]
    public string UserId { get; set; }

    [Column("WorkoutTypeId")]
    [Required]
    public int WorkoutTypeId { get; set; }

    [Column("Date")]
    [Required]
    public DateTime Date { get; set; }

    [Column("SixCount")]
    public int? SixCount { get; set; }

    [Column("NavySeal")]
    public int? NavySeal { get; set; }

    [Column("Comment")]
    public string? Comment { get; set; }

    public ICollection<WorkoutValuesEntity> WokoutValues { get; set; }

    [Column("WorkoutTime")]
    [Required]
    public decimal WorkoutTime { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }

    [ForeignKey(nameof(WorkoutTypeId))]
    public WorkoutTypeEntity WorkoutType { get; set; }
}
