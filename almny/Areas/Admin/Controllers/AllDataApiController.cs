using almny.Areas.Admin.Models.DTO;
using almny.Models;
using almny.Repositories.Abstract;
using almny.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AllDataApiController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public AllDataApiController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            if (_context.Subjects == null)
            {
                return NotFound();
            }
            return await _context.Subjects.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            return await _context.Videos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Video>>> GetFilteredVideos(int? subject, int? level, int? dept, int? semester)
        {
            var query = _context.Videos.AsQueryable();

            // Combine conditions using && (logical AND) operator
            query = query.Where(v =>
                (!subject.HasValue || v.SubjectId == subject.Value) &&
                (!level.HasValue || v.LevelId == level.Value) &&
                (!dept.HasValue || v.DepartmentId == dept.Value) &&
                (!semester.HasValue || v.SemesterId == semester.Value ||
                v.SemesterId == 3)
            );

            var result = await query.ToListAsync();
            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            return await _context.Books.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Book>>> GetFilteredBooks(int? subject, int? level, int? dept, int? semester)
        {
            var query = _context.Books.AsQueryable();

            // Combine conditions using && (logical AND) operator
            query = query.Where(v =>
                (!subject.HasValue || v.SubjectId == subject.Value) &&
                (!level.HasValue || v.LevelId == level.Value) &&
                (!dept.HasValue || v.DepartmentId == dept.Value) &&
                (!semester.HasValue || v.SemesterId == semester.Value ||
                v.SemesterId == 3)
            );

            var result = await query.ToListAsync();
            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetVideoComments(int videoId)
        {
            var comments = await _context.Comments
                .Where(c => c.VideoId == videoId)
                .Include(c => c.User)
                .Select(c => new CommentDto
                {
                    CommentId = c.Id,
                    CommentText = c.comment,
                    UserName = c.User.Name,
                    UserImage = c.User.ProfilePicture,
                    UserId = c.UserId,
                    CommentDate = c.date.ToString("dd-MM-yyyy")
                })
                .ToListAsync();

            return comments;
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



            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<CommentDto>> UpdateComment(int commentId, string updatedComment)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound(); // Comment not found
            }

            // Update the comment content
            comment.comment = updatedComment;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound(); // Comment not found
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent(); // Comment deleted successfully
        }

        [HttpPost]
        public async Task<ActionResult<Like>> LikeVideo(int videoId)
        {
            var userId = ((await _userManager.GetUserAsync(User)).Id);
            var like = new Like
            {
                UserId = userId,
                VideoId = videoId
            };

            _context.Likes.Update(like);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<Like>> UnlikeVideo(int videoId)
        {
            var userId = ((await _userManager.GetUserAsync(User)).Id);

            var like = _context.Likes.FirstOrDefault(x => x.UserId == userId && x.VideoId == videoId);
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
