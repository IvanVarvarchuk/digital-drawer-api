using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Application.Common.Models
{
    public class LoginUserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        //[StringLength(25, ErrorMessage = "Password is limited to {2} to {1} characters", MinimumLength = 10)]
        public string Password { get; set; }
    }
}
