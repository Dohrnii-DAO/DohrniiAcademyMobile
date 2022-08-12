using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Models.UserModels;
using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DohrniiFoundation.Services
{
    public class CacheService : ICacheService
    {
        public async Task<LoginResponse> GetCurrentUser()
        {
            try
            {
                //await Task.Delay(10);
                var cacheState = Barrel.Current.Get<LoginResponse>(key: StringConstant.CurrentUserKey);
                return cacheState;
            }
            catch (Exception ex)
            {
                var dd = ex.Message;
            }
            return null;
        }

        public async Task RemoveCurrentUser()
        {
            //await Task.Delay(10);
            Barrel.Current.Empty(key: StringConstant.CurrentUserKey);
        }

        public async Task SaveCurrentUser(LoginResponse loginResponse)
        {
            //await Task.Delay(10);
            Barrel.Current.Empty(key: StringConstant.CurrentUserKey);
            Barrel.Current.Add(key: StringConstant.CurrentUserKey, data: loginResponse, expireIn: TimeSpan.FromDays(StringConstant.CacheDuration));
        }

        public async Task SaveCategoryProgress(CategoryProgress data)
        {
            //await Task.Delay(10);
            Barrel.Current.Empty(key: $"catId_{data.CategoryId}");
            Barrel.Current.Add(key: $"catId_{data.CategoryId}", data: data, expireIn: TimeSpan.FromDays(StringConstant.CacheDuration));
        }
        public async Task<CategoryProgress> GetCategoryProgress(string categoryId)
        {
            //await Task.Delay(10);
            var cacheState = Barrel.Current.Get<CategoryProgress>(key: $"catId_{categoryId}");
            return cacheState;
        }

        public async Task SaveUserStatus(UserStatus data)
        {
            //await Task.Delay(10);
            Barrel.Current.Empty(key: "userstatus");
            Barrel.Current.Add(key: "userstatus", data: data, expireIn: TimeSpan.FromDays(StringConstant.CacheDuration));
        }
        public async Task<UserStatus> GetUserStatus()
        {
            //await Task.Delay(10);
            var cacheState = Barrel.Current.Get<UserStatus>(key: $"userstatus");
            return cacheState;
        }

        public async Task SaveChapterDetail(ChapterModel data)
        {
            //await Task.Delay(10);
            Barrel.Current.Empty(key: $"chapter_{data.Id}");
            Barrel.Current.Add(key: $"chapter_{data.Id}", data: data, expireIn: TimeSpan.FromDays(StringConstant.CacheDuration));
        }

        public async Task<ChapterModel> GetChapterDetail(string chapterId)
        {
            //await Task.Delay(10);
            var cacheState = Barrel.Current.Get<ChapterModel>(key: $"chapter_{chapterId}");
            return cacheState;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            //await Task.Delay(10);
            var cacheState = Barrel.Current.Get<List<CategoryModel>>(key: "categories");
            return cacheState;
        }

        public async Task SaveCategories(List<CategoryModel> data)
        {
            //await Task.Delay(10);
            Barrel.Current.Empty(key: "categories");
            Barrel.Current.Add(key: "categories", data: data, expireIn: TimeSpan.FromDays(StringConstant.CacheDuration));
        }

        public async Task Logout()
        {
            Barrel.Current.EmptyAll();
        }
    }
}
