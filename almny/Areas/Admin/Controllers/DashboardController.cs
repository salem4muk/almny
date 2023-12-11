using almny.Areas.Admin.Models;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {

        private readonly DatabaseContext _context;

        public DashboardController(DatabaseContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var CountExams = 0;
            var CountUsers = 0;

            DashboardCountsModel countsModel = new DashboardCountsModel
            {
                CountSubjects = _context.Subjects.Count(),
                CountVideos = _context.Videos.Count(),
                CountBooks = _context.Books.Count(),
                CountExams = CountExams,
                CountLikes = _context.Likes.Count(),
                CountUsers = CountUsers,
                CountComments = _context.Comments.Count(),
            };

            return View(countsModel);
        }
    }
}
