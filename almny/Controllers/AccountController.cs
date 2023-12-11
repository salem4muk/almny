using almny.Models.DTO;
using almny.Repositories.Abstract;
using almny.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace almny.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IUserAuthenticationService authService, DatabaseContext context,IWebHostEnvironment _webHostEnvironment)
        {
            this._authService = authService;
            this._context = context;
            this._webHostEnvironment = _webHostEnvironment;
        }

        
        public IActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);
            if(result.StatusCode==1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }
       
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if(!ModelState.IsValid) { return View(model); }
            var file = HttpContext.Request.Form.Files;
            if (file.Count() > 0)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var fileStream = new FileStream(Path.Combine(@"wwwroot/Uploads/images", ImageName), FileMode.Create);
                file[0].CopyTo(fileStream);
                model.ProfilePicture = ImageName;
            }

            model.Role = "user";
            var result = await this._authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();  
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
              return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

        [Authorize]
        public IActionResult Profile(string id)
        {
            TempData["UserLikesCount"] = _context.Likes.Count(x => x.UserId == id);
            TempData["UserCommentsCount"] = _context.Comments.Count(x => x.UserId == id); ;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            var user = await this._authService.UpdateProfileGetDataAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateModel model,string id)
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

            var result = await _authService.UpdateProfileAsync(model,id);
            if (result.StatusCode == 1)
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Update));
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Update));
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser()
        {
            RegistrationModel model = new RegistrationModel
            {
                Email = "user@gmail.com",
                Name = "User Test",
                Password = "123456"
            };
            model.Role = "user";
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }

    }
}
