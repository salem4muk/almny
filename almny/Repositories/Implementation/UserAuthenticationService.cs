using almny.Models;
using almny.Models.DTO;
using almny.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace almny.Repositories.Implementation
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager; 

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "المستخدم موجود بالفعل";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Name = model.Name,
                ProfilePicture = model.ProfilePicture,
                EmailConfirmed=true,
                PhoneNumberConfirmed=true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "فشل إنشاء المستخدم";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "لقد قمت بالتسجيل بنجاح";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "البريد الإلكتروني غير صالح";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "كلمة المرور غير صالحة";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "تم تسجيل الدخول بنجاح";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "تم حظر المستخدم";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "خطأ في تسجيل الدخول";
            }
           
            return status;
        }

        public async Task LogoutAsync()
        {
           await signInManager.SignOutAsync();
           
        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model,string username)
        {
            var status = new Status();
            
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "المستخدم غير موجود";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "تم تحديث كلمة السر بنجاح";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "حدث بعض الخطأ";
                status.StatusCode = 0;
            }
            return status;

        }

        public async Task<UpdateModel> UpdateProfileGetDataAsync(string id)
        {
            var User = await userManager.FindByIdAsync(id);
            var updateModel = new UpdateModel
            {
                Id = User.Id,
                Name = User.Name,
                Email = User.Email,
                ProfilePicture = User.ProfilePicture,
            };
            return updateModel;
        }

        public async Task<Status> UpdateProfileAsync(UpdateModel model, string id)
        {
            var status = new Status();

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                status.Message = "المستخدم غير موجود";
                status.StatusCode = 0;
                return status;
            }
            user.Name = model.Name;
            user.Email = model.Email;
            user.ProfilePicture = model.ProfilePicture;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                status.Message = "تم التحديث بنجاح";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "حدث بعض الخطأ";
                status.StatusCode = 0;
            }
            return status;
        }
    }
}
