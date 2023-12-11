using System.ComponentModel.DataAnnotations;

namespace almny.Areas.Admin.Models.DTO
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال اسم المادة")]
        public string Name { get; set; }
    }
}
