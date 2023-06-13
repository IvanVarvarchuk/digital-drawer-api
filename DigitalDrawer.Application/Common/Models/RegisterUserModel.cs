using System.ComponentModel.DataAnnotations;

namespace DigitalDrawer.Application.Common.Models;

public class RegisterUserModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
}
