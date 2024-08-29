using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdtShared.Entities;

[Table("Weekdays")]
public class WeekdayEntity : IEntity<int>
{
    [Column("Id")]
    [Key]
    public int Id { get; set; }

    [Column("Name")]
    [Required]
    public string Name { get; set; }
}
