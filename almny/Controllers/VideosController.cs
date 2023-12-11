using almny.Models;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace almny.Controllers
{
    [Authorize]
    public class VideosController : Controller
{

        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public VideosController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
    {
        return View();
    }

        public async Task<IActionResult> Details(int id)
        {
            var userId = ((await _userManager.GetUserAsync(User)).Id);
            var video = _context.Videos.FirstOrDefault(x => x.Id == id);

            ViewData["UserId"] = userId;
            ViewData["IsLiked"] = IsLiked(userId, id);
            ViewData["UserLikesCount"] = _context.Likes.Count(u => u.VideoId == id);
            return View(video);
        }

        public bool IsLiked(string userId, int videoId)
        {
            bool IsLiked = false;
            var like = _context.Likes.FirstOrDefault(l => l.VideoId == videoId & l.UserId == userId);
            if (like != null)
            {
                IsLiked = true;
            }

            return IsLiked;
        }

    }
}
