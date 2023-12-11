using System.ComponentModel.DataAnnotations;

namespace almny.Models.DTO
{
    public class ChangePasswordModel
    {
        [Required]
        public string ? CurrentPassword { get; set; }

        [Required]
        public string? NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ? PasswordConfirm { get; set; }
        
    }
}
