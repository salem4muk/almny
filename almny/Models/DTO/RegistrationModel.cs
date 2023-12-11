using System.ComponentModel.DataAnnotations;

namespace almny.Models.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "الرجاء ادخال اسم المستخدم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال البريد الاكتروني")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال كلمة السر")]
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$",ErrorMessage ="Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة")]
        public string PasswordConfirm { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Role { get; set; }
        
    }
}
