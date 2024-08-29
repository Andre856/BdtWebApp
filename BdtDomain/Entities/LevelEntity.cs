using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BdtShared.Entities;

[Table("Levels")]
public class LevelEntity : IEntity<int>
{
    [Column("Id")]
    [Key]
    public int Id { get; set; }

    [Column("Level")]
    [Required]
    public string Level { get; set; }

    [Column("Lower6Count")]
    [Required]
    public int Lower6Count { get; set; }

    [Column("Upper6Count")]
    [Required]
    public int Upper6Count { get; set; }

    [Column("LowerNavySeal")]
    [Required]
    public int LowerNavySeal { get; set; }

    [Column("UpperNavySeal")]
    [Required]
    public int UpperNavySeal { get; set; }
}
