using almny.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace almny.Controllers
{
    public class BooksController : Controller
{

        private readonly DatabaseContext _context;

        public BooksController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
    {

        return View();
    }


        public async Task<IActionResult> Details(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            return View(book);
        }
    }
}
