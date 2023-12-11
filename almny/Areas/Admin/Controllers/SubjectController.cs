using almny.Areas.Admin.Models.DTO;
using almny.Models;
using almny.Repositories.Abstract;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    [Authorize(Roles = "admin")]
    public class SubjectController : Controller
    {
        private readonly IBaseRepository<Subject> _subjectRepository;

        public SubjectController(IBaseRepository<Subject> subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        [HttpGet("[action]")]
        public IActionResult Index()
        {
            // قم بالحصول على جميع المواضيع باستخدام المخزن
            var subjects = _subjectRepository.GetAll().ToList();

            // قم بتمرير البيانات إلى العرض
            return View(subjects);
        }

        [HttpGet("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult Create(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                // قم بتحويل SubjectViewModel إلى كائن Subject
                var subject = new Subject
                {
                    // تعيين الخصائص بناءً على الموديل
                    // على سبيل المثال، افترض أن SubjectViewModel لديها خاصية SubjectName
                    Name = model.Name
                };

                // قم بإنشاء الموضوع باستخدام المخزن
                bool created = _subjectRepository.Create(subject);

                if (created)
                {
                    // يمكنك توجيه المستخدم إلى صفحة نجاح أو فعل آخر بناءً على الحاجة
                    return RedirectToAction("Index");
                }
                else
                {
                    // إذا فشلت العملية، يمكنك إعادة عرض النموذج مع رسالة خطأ
                    ModelState.AddModelError(string.Empty, "فشل في إنشاء الموضوع.");
                    return View(model);
                }
            }

            // إذا كان هناك أخطاء في نموذج الإدخال، قم بإعادة عرض النموذج
            return View(model);
        }

        [HttpGet("[action]")]
        public IActionResult Edit(int id)
        {
            // قم بالبحث عن الكائن المطلوب باستخدام المعرف
            var subject = _subjectRepository.FindByid(id);

            if (subject == null)
            {
                // إذا لم يتم العثور على الكائن، قم بإعادة توجيه المستخدم إلى صفحة خطأ أو صفحة الفهرس أو أي صفحة أخرى
                return RedirectToAction("Index");
            }

            // قم بتحويل الكائن إلى نموذج التحرير (SubjectViewModel) إذا كان ذلك ضروريًا
            var model = new SubjectViewModel
            {
                // قم بتعيين الخصائص بناءً على الكائن
                // على سبيل المثال، افترض أن SubjectViewModel لديها خاصية SubjectName
                Id = subject.Id,
                Name = subject.Name
            };

            // قم بإعادة عرض نموذج التحرير مع البيانات المحملة
            return View(model);
        }

        [HttpPost("[action]")]
        public IActionResult Edit(SubjectViewModel model,int id)
        {
            if (ModelState.IsValid)
            {
                // قم بالبحث عن الكائن المطلوب باستخدام المعرف
                var existingSubject = _subjectRepository.FindByid(model.Id);

                if (existingSubject == null)
                {
                    // إذا لم يتم العثور على الكائن، قم بإعادة توجيه المستخدم إلى صفحة خطأ أو صفحة الفهرس أو أي صفحة أخرى
                    return RedirectToAction("Index");
                }

                // تحديث الخصائص بناءً على نموذج التحرير
                // على سبيل المثال، افترض أن SubjectViewModel لديها خاصية SubjectName
                existingSubject.Name = model.Name;

                // قم بتحديث الكائن باستخدام المخزن
                bool updated = _subjectRepository.Update(existingSubject);

                if (updated)
                {
                    // في حال نجاح التحديث، قم بتوجيه المستخدم إلى صفحة الفهرس أو أي صفحة أخرى
                    return RedirectToAction("Index");
                }
                else
                {
                    // إذا فشل التحديث، قم بإعادة عرض نموذج التحرير مع رسالة خطأ
                    ModelState.AddModelError(string.Empty, "فشل في تحديث الموضوع.");
                    return View(model);
                }
            }

            // إذا كان هناك أخطاء في نموذج التحرير، قم بإعادة عرض النموذج مع الأخطاء
            return View(model);
        }

        [HttpGet("[action]")]
        public IActionResult Delete(int id)
        {
            var entityToDelete = _subjectRepository.FindByid(id);

            if (entityToDelete != null)
            {
                bool deleted = _subjectRepository.Delete(entityToDelete);

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
