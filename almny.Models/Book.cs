using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace almny.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string book { get; set; }
        public string thumb { get; set; }
        public int SubjectId { get; set; }
        public int LevelId { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }


        public virtual Subject Subject { get; set; }
        public virtual Level Level { get; set; }
        public virtual Department Department { get; set; }
        public virtual Semester Semester { get; set; }

    }
}
