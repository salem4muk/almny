using almny.Areas.Admin.Models;
using almny.Areas.Admin.Models.DTO;
using almny.Models;
using almny.Repositories.Abstract;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    public class BookController : Controller
    {
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBaseRepository<Book> bookRepository, DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int? LevelId, int? DepartmentId,int? SemesterId, int? SubjectId)
        {
            List<Book> books = _bookRepository
                .GetAll()
                .Include(b => b.Department)
                .Include(b => b.Level)
                .Include(b => b.Semester)
                .Include(b => b.Subject)
                .Where(b =>
        (!LevelId.HasValue || b.LevelId == LevelId) &&
        (!DepartmentId.HasValue || b.DepartmentId == DepartmentId) &&
        (!SemesterId.HasValue || b.SemesterId == SemesterId) &&
        (!SubjectId.HasValue || b.SubjectId == SubjectId))
    .ToList();

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(books);
        }

        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    if (!string.IsNullOrEmpty(model.thumb))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", model.thumb);
                        string oldBookPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/books", model.book);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            System.IO.File.Delete(oldBookPath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", imageName);
                    string bookName = Guid.NewGuid().ToString() + Path.GetExtension(files[1].FileName);
                    string bookPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/books", bookName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    using (var fileStream = new FileStream(bookPath, FileMode.Create))
                    {
                        files[1].CopyTo(fileStream);
                    }

                    model.thumb = imageName;
                    model.book = bookName;
                }
                var book = new Book
                {
                    // تعيين الخصائص بناءً على الموديل
                    Title = model.Title,
                    book = model.book,
                    thumb = model.thumb,
                    SubjectId = model.SubjectId,
                    LevelId = model.LevelId,
                    DepartmentId = model.DepartmentId,
                    SemesterId = model.SemesterId,
                };

                // قم بإنشاء الموضوع باستخدام المخزن
                bool created = _bookRepository.Create(book);

                if (created)
                {
                    // يمكنك توجيه المستخدم إلى صفحة نجاح أو فعل آخر بناءً على الحاجة
                    return RedirectToAction("Index");
                }
                else
                {
                    // إذا فشلت العملية، يمكنك إعادة عرض النموذج مع رسالة خطأ
                    ModelState.AddModelError(string.Empty, "فشل في إنشاء الموضوع.");
                    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
                    ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
                    ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
                    ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
                    return View(model);
                }
            }

            ModelState.AddModelError(string.Empty, "الرجاء ادخال كافة العناصر المطلوبة");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var book = _bookRepository.FindByid(id);

            if (book == null)
            {
                return RedirectToAction("Index");
            }

            var model = new BookViewModel
            {
                // قم بتعيين الخصائص بناءً على الكائن
                Id = book.Id,
                Title = book.Title,
                book = book.book,
                thumb = book.thumb,
                SubjectId = book.SubjectId,
                LevelId = book.LevelId,
                DepartmentId = book.DepartmentId,
                SemesterId = book.SemesterId
            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    if (!string.IsNullOrEmpty(model.thumb))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", model.thumb);
                        string oldBookPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/books", model.book);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            System.IO.File.Delete(oldBookPath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", imageName);
                    string bookName = Guid.NewGuid().ToString() + Path.GetExtension(files[1].FileName);
                    string bookPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/books", bookName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    using (var fileStream = new FileStream(bookPath, FileMode.Create))
                    {
                        files[1].CopyTo(fileStream);
                    }

                    model.thumb = imageName;
                    model.book = bookName;
                }

                // استرجاع كائن الكتاب الحالي من المستودع
                var existingbook = _bookRepository.FindByid(id);

                if (existingbook == null)
                {
                    // إذا لم يتم العثور على الكتاب، قم بإعادة توجيه المستخدم إلى صفحة خطأ أو صفحة الفهرس أو أي صفحة أخرى
                    return RedirectToAction("Index");
                }

                // تحديث كائن الكتاب الحالي بقيم من نموذج العرض
                existingbook.Title = model.Title;
                existingbook.book = model.book;
                existingbook.thumb = model.thumb;
                existingbook.SubjectId = model.SubjectId;
                existingbook.LevelId = model.LevelId;
                existingbook.DepartmentId = model.DepartmentId;
                existingbook.SemesterId = model.SemesterId;

                // استدعاء طريقة التحديث في المستودع لحفظ التغييرات
                bool updated = _bookRepository.Update(existingbook);

                if (updated)
                {
                    // إذا نجح التحديث، قم بإعادة توجيه المستخدم إلى صفحة الفهرس
                    return RedirectToAction("Index");
                }
                else
                {
                    // إذا فشل التحديث، قد ترغب في التعامل مع ذلك بشكل مناسب (مثل عرض رسالة خطأ)
                    ModelState.AddModelError(string.Empty, "فشل في تحديث الفيديو. يرجى المحاولة مرة أخرى.");
                    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
                    ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
                    ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
                    ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
                    return View(model);
                }
            }

            // إذا لم تكن حالة النموذج صحيحة، أعد عرض صفحة التحرير مع أخطاء التحقق
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(model);
        }


        public IActionResult Delete(int id)
        {
            var entityToDelete = _bookRepository.FindByid(id);

            if (entityToDelete != null)
            {
                if (!string.IsNullOrEmpty(entityToDelete.thumb))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", entityToDelete.thumb);
                    string oldBookPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/books", entityToDelete.book);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                        System.IO.File.Delete(oldBookPath);
                    }
                }
                bool deleted = _bookRepository.Delete(entityToDelete);

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
