using almny.Areas.Admin.Models.DTO;
using almny.Models;
using almny.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace almny.Controllers
{
    public class CommentsController : Controller
    {

        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CommentsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> IndexAsync()
        {
            var userId = ((await _userManager.GetUserAsync(User)).Id);

            var comments = _context.Comments
                .Where(x => x.UserId == userId)
                .Include(v => v.Video);
            return View(comments);
        }


        [HttpPost]
        public async Task<ActionResult<CommentDto>> AddComment(int videoId, string commentText)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return a 400 Bad Request if the model is not valid
            }

            var userId = ((await _userManager.GetUserAsync(User)).Id);

            var comment = new Comment
            {
                UserId = userId,
                VideoId = videoId,
                comment = commentText,
                date = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();



            return RedirectToAction("Details", "Videos", new { id = videoId });
        }

    }
}
