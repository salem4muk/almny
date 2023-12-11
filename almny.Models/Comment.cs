using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace almny.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string comment { get; set; }
        public string UserId { get; set; }
        public int VideoId { get; set; }
        public DateTime date { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual Video Video { get; set; }

    }
}
