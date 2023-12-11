using almny.Areas.Admin.Models.DTO;
using almny.Models;
using almny.Repositories.Abstract;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    public class VideoController : Controller
    {
        private readonly IBaseRepository<Video> _videoRepository;
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideoController(IBaseRepository<Video> videoRepository, DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _videoRepository = videoRepository;
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index(int? LevelId, int? DepartmentId, int? SemesterId, int? SubjectId)
        {
            List<Video> videos = _videoRepository
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
            return View(videos);
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
        public IActionResult Create(VideoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    if (!string.IsNullOrEmpty(model.thumb))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", model.thumb);
                        string oldVideoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/videos", model.video);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            System.IO.File.Delete(oldVideoPath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", imageName);
                    string videoName = Guid.NewGuid().ToString() + Path.GetExtension(files[1].FileName);
                    string videoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/videos", videoName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    using (var fileStream = new FileStream(videoPath, FileMode.Create))
                    {
                        files[1].CopyTo(fileStream);
                    }

                    model.thumb = imageName;
                    model.video = videoName;
                }
                var video = new Video
                {
                    // تعيين الخصائص بناءً على الموديل
                    Title = model.Title,
                    video = model.video,
                    thumb = model.thumb,
                    SubjectId = model.SubjectId,
                    LevelId = model.LevelId,
                    DepartmentId = model.DepartmentId,
                    SemesterId = model.SemesterId,
                    date = DateTime.Now
                };

                // قم بإنشاء الموضوع باستخدام المخزن
                bool created = _videoRepository.Create(video);

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
            var video = _videoRepository.FindByid(id);

            if (video == null)
            {
                // إذا لم يتم العثور على الكائن، قم بإعادة توجيه المستخدم إلى صفحة خطأ أو صفحة الفهرس أو أي صفحة أخرى
                return RedirectToAction("Index");
            }

            // قم بتحويل الكائن إلى نموذج التحرير (SubjectViewModel) إذا كان ذلك ضروريًا
            var model = new VideoViewModel
            {
                // قم بتعيين الخصائص بناءً على الكائن
                // على سبيل المثال، افترض أن SubjectViewModel لديها خاصية SubjectName
                Id = video.Id,
                Title = video.Title,
                video = video.video,
                thumb = video.thumb,
                SubjectId = video.SubjectId,
                LevelId = video.LevelId,
                DepartmentId = video.DepartmentId,
                SemesterId = video.SemesterId
            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["LevelId"] = new SelectList(_context.Levels, "Id", "Name");
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(VideoViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    if (!string.IsNullOrEmpty(model.thumb))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", model.thumb);
                        string oldVideoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/videos", model.video);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            System.IO.File.Delete(oldVideoPath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", imageName);
                    string videoName = Guid.NewGuid().ToString() + Path.GetExtension(files[1].FileName);
                    string videoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/videos", videoName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    using (var fileStream = new FileStream(videoPath, FileMode.Create))
                    {
                        files[1].CopyTo(fileStream);
                    }

                    model.thumb = imageName;
                    model.video = videoName;
                }

                // استرجاع كائن الفيديو الحالي من المستودع
                var existingVideo = _videoRepository.FindByid(id);

                if (existingVideo == null)
                {
                    // إذا لم يتم العثور على الفيديو، قم بإعادة توجيه المستخدم إلى صفحة خطأ أو صفحة الفهرس أو أي صفحة أخرى
                    return RedirectToAction("Index");
                }

                // تحديث كائن الفيديو الحالي بقيم من نموذج العرض
                existingVideo.Title = model.Title;
                existingVideo.video = model.video;
                existingVideo.thumb = model.thumb;
                existingVideo.SubjectId = model.SubjectId;
                existingVideo.LevelId = model.LevelId;
                existingVideo.DepartmentId = model.DepartmentId;
                existingVideo.SemesterId = model.SemesterId;
                existingVideo.date = DateTime.Now;

                // استدعاء طريقة التحديث في المستودع لحفظ التغييرات
                bool updated = _videoRepository.Update(existingVideo);

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
            var entityToDelete = _videoRepository.FindByid(id);

            if (entityToDelete != null)
            {
                if (!string.IsNullOrEmpty(entityToDelete.thumb))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", entityToDelete.thumb);
                    string oldVideoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/videos", entityToDelete.video);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                        System.IO.File.Delete(oldVideoPath);
                    }
                }
                bool deleted = _videoRepository.Delete(entityToDelete);

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
