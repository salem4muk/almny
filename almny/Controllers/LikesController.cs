using almny.Models;
using almny.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace almny.Controllers
{
    public class LikesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public LikesController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var userId = ((await _userManager.GetUserAsync(User)).Id);

            var likes = _context.Likes
                .Where(x => x.UserId == userId)
                .Include(v => v.Video);
            return View(likes);
        }
    }
}
