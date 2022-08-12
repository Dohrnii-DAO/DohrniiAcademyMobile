using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DohrniiFoundation.IServices
{
    public interface ICacheService
    {
        Task SaveCurrentUser(LoginResponse loginResponse);
        Task<LoginResponse> GetCurrentUser();
        Task RemoveCurrentUser();

        Task SaveCategoryProgress(CategoryProgress data);
        Task<CategoryProgress> GetCategoryProgress(string categoryId);

        Task SaveUserStatus(UserStatus data);
        Task<UserStatus> GetUserStatus();

        Task SaveChapterDetail(ChapterModel data);
        Task<ChapterModel> GetChapterDetail(string chapterId);
        Task<List<CategoryModel>> GetCategories();
        Task SaveCategories(List<CategoryModel> data);
        Task Logout();
    }
}
