using almny.Models;
using almny.Repositories.Abstract;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    public class CommentController : Controller
    {

        private readonly IBaseRepository<Comment> _commentRepository;
        private readonly DatabaseContext _context;

        public CommentController(IBaseRepository<Comment> commentRepository, DatabaseContext context)
        {
            _commentRepository = commentRepository;
            _context = context;
        }

        public IActionResult Index(int? VideoId)
        {
          
            List<Comment> comments = _commentRepository
           .GetAll()
           .Include(b => b.Video)
           .Include(b => b.User)
           .Where(b => (!VideoId.HasValue || b.VideoId == VideoId))
           .ToList();

            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Title");
            return View(comments);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var entityToDelete = _commentRepository.FindByid(id);

            if (entityToDelete != null)
            {
                bool deleted = _commentRepository.Delete(entityToDelete);

                if (deleted)
                {
                    // في حال نجاح الحذف، يمكنك توجيه المستخدم إلى صفحة الفهرس أو أي صفحة أخرى
                    return RedirectToAction("Index");
                }
            }

            // في حالة فشل الحذف، يمكنك إرجاع رسالة خطأ أو توجيه المستخدم إلى صفحة الفهرس أو أي صفحة أخرى
            return RedirectToAction("Index");
        }

    }
}
