using System.ComponentModel.DataAnnotations;

namespace BdtShared.Models.App;

public class ResetPasswordModel
{
    [Required] public string Token { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Email { get; set; }
}
