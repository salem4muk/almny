using almny.Models.DTO;
using almny.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace almny.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]")]
    public class AccountController : Controller
{

        private readonly IUserAuthenticationService _authService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IUserAuthenticationService authService, IWebHostEnvironment _webHostEnvironment)
        {
            this._authService = authService;
            this._webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet("[action]")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            var user = await this._authService.UpdateProfileGetDataAsync(id);
            return View(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(UpdateModel model, string id)
        {
            if (!ModelState.IsValid)
                return View(model);
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                if (!string.IsNullOrEmpty(model.ProfilePicture))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", model.ProfilePicture);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads/images", imageName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                model.ProfilePicture = imageName;
            }
            var result = await _authService.UpdateProfileAsync(model, id);
            if (result.StatusCode == 1)
            {
                TempData["msg"] = result.Message;
                return RedirectToAction("Update", new { id = id });
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction("Update", new { id = id });
            }
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin()
        {
            RegistrationModel model = new RegistrationModel
            {
                Email = "admin@gmail.com",
                Name = "Admin Test",
                Password = "123456"
            };
            model.Role = "admin";
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
