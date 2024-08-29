using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdtShared.Entities;

[Table("Planner")]
public class PlannerEntity : IEntity<Guid>, IUserEntity
{
    [Column("Id")]
    [Key]
    public Guid Id { get; set; }

    [Column("UserId")]
    [Required]
    public string UserId { get; set; }

    [Column("WeekDayId")]
    [Required]
    public int WeekDayId { get; set; }

    [Column("WorkoutTypeId")]
    [Required]
    public int WorkoutTypeId { get; set; }

    [Column("WorkoutDuration")]
    [Required]
    public decimal WorkoutDuration { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }

    [ForeignKey(nameof(WeekDayId))]
    public WeekdayEntity Weekdays { get; set; }

    [ForeignKey(nameof(WorkoutTypeId))]
    public WorkoutTypeEntity WorkoutType { get; set; }
}
