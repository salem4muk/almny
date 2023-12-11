using System.ComponentModel.DataAnnotations;

namespace almny.Models.DTO
{
    public class UpdateModel
{
        public string Id { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال اسم المستخدم")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "الرجاء ادخال البريد الاكتروني")]
        public string Email { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
