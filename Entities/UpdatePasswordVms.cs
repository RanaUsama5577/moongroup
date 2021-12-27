using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Entities
{
    public class UpdatePasswordVms
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string current_password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string new_password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("new_password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string confirm_password { get; set; }
    }
}