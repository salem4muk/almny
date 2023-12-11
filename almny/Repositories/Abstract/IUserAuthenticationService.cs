using almny.Models.DTO;

namespace almny.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
   
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
        Task<UpdateModel> UpdateProfileGetDataAsync(string id);
        Task<Status> UpdateProfileAsync(UpdateModel model,string id);
    }
}
