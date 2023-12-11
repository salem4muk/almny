using almny.Areas.Admin.Models.DTO;
using almny.Models;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    public class LikeController : Controller
    {

        private readonly DatabaseContext _context;

        public LikeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var videoTableData = _context.Videos
                .Select(v => new VideoLikeViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    Thumbnail = v.thumb,
                    LikesCount = _context.Likes.Count(like => like.VideoId == v.Id)
                }).ToList();
                
            return View(videoTableData);
        }
    }
}
