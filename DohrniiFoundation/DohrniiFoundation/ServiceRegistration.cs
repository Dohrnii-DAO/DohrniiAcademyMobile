using DohrniiFoundation.IServices;
using DohrniiFoundation.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DohrniiFoundation
{
    public class ServiceRegistration
    {
        public static void RegisterServices(App app)
        {
            DependencyService.Register<ILessonService, LessonService>();
            DependencyService.Register<IUserService, UserService>();
            DependencyService.Register<ISocialService, SocialService>();
            DependencyService.Register<ICacheService, CacheService>();
            DependencyService.Register<IAppState, AppState>();
        }
    }
}
