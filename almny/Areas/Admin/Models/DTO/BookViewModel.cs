using almny.Models;
using System.ComponentModel.DataAnnotations;

namespace almny.Areas.Admin.Models.DTO
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال عنوان الكتاب")]
        public string Title { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار الكتاب")]
        public string book { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار صورة مصغرة")]
        public string thumb { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المستوى الدراسي")]
        public int LevelId { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار القسم")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار الفصل الدراسي")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المادة الدراسي")]
        public int SubjectId { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual Level? Level { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
