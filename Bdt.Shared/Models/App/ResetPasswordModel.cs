using System.ComponentModel.DataAnnotations;

namespace Bdt.Shared.Models.App;

public class ResetPasswordModel
{
    [Required] public string Token { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Email { get; set; }
}
