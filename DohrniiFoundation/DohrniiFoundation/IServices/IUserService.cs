using DohrniiFoundation.Models.APIRequestModel;
using DohrniiFoundation.Models.APIRequestModel.User;
using DohrniiFoundation.Models.Socials;
using DohrniiFoundation.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DohrniiFoundation.IServices
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequestModel loginRequest);
        Task<List<AppUser>> GetUsers();
        Task<AppUser> GetUsers(int id);
        Task<AppUser> GetAppUser();
        Task<RegisterModel> Register(RegisterModel model);
        Task<LoginResponse> ValidateUser(UserValidationModel model);
        Task<User> UpdateProfile(UpdateProfile model);
        Task<ChangePasswordResp> ChangePassword(ChangePasswordRequestModel model);
    }
}
