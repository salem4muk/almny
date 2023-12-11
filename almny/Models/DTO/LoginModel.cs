using System.ComponentModel.DataAnnotations;

namespace almny.Models.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "الرجاء ادخال البريد الاكتروني")]
        public string Email { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال كلمة السر")]
        public string Password { get; set; }
    }
}
