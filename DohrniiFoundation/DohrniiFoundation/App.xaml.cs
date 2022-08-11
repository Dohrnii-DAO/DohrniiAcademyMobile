using System;
using System.Threading.Tasks;
using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.Lessons;
using DohrniiFoundation.Views.User;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.FileStore;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

[assembly: ExportFont("futur.ttf", Alias = "Futur")]
[assembly: ExportFont("Futura Bold font.ttf", Alias = "Bold")]
[assembly: ExportFont("Futura Book font.ttf", Alias = "Book")]
[assembly: ExportFont("FuturaPTDemi.otf", Alias = "PTDemi")]
[assembly: ExportFont("FuturaPTBook.otf", Alias = "PTBook")]
[assembly: ExportFont("FuturaMediumBT.ttf", Alias = "BTMedium")]
[assembly: ExportFont("MonumentExtended-Regular.otf", Alias = "MonumentRegular")]
[assembly: ExportFont("Poppins-Regular.ttf", Alias = "PoppinsRegular")]
[assembly: ExportFont("Poppins-SemiBold.ttf", Alias = "PoppinsSemiBold")]
[assembly: ExportFont("Poppins-Medium.ttf", Alias = "PoppinsMedium500")]
[assembly: ExportFont("Poppins-LightItalic.ttf", Alias = "PoppinsItalic300")]

namespace DohrniiFoundation
{
    public partial class App : Application
    {
        private readonly ICacheService _cacheService;

        #region Constructor
        public App()
        {
            Device.SetFlags(new string[] { "MediaElement_Experimental", "IndicatorView_Experimental", "Markup_Experimental", "Brush_Experimental", "Shapes_Experimental" });
            InitializeComponent();
            Barrel.ApplicationId = "DohrniiAcademyApp";
            ServiceRegistration.RegisterServices(this);
            Preferences.Set(StringConstant.TabIdKey, 2);
            _cacheService = DependencyService.Get<ICacheService>(); //new CacheService();
            ShowPage();
        }

        async Task ShowPage()
        {
            if (Preferences.Get(StringConstant.IsUsed, false))
            {
                var login = await _cacheService.GetCurrentUser();
                if (login != null)
                {
                    if (DateTime.Now < login.ExpiringDate)
                    {
                        AppUtil.CurrentUser = login.User;
                        MainPage = new NavigationPage(new LessonsPage());
                    }
                    else
                    {
                        MainPage = new NavigationPage(new LoginPage());
                    }
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            else
            {
                MainPage = MainPage = new NavigationPage(new OnboardingPage());
            }
        }

        #endregion


        #region Methods
        /// <summary>
        /// On start to track the app is installed on device
        /// </summary>
        protected override void OnStart()
        {
            // Setup App Center
            AppCenter.Start($"ios=a012ea5a-c494-44bb-9143-897bcec5ecc3;android=4b7cf062-a4f3-42e2-8e72-a431d6839bba", typeof(Analytics), typeof(Crashes));
            VersionTracking.Track();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        /// <summary>
        /// Method is used to get the app link to reset the password
        /// </summary>
        /// <param name="uri"></param>
        protected async override void OnAppLinkRequestReceived(Uri uri)
        {
            try
            {
                if (uri.Host.EndsWith(StringConstant.DeepLinkingAPIKeyURL, StringComparison.OrdinalIgnoreCase))
                {
                    if (uri != null && uri.Segments != null)
                    {
                        string[] uriString = uri.OriginalString.Split('=');
                        if (uri.Segments[1] == StringConstant.reset_password)
                        {
                            Preferences.Set(StringConstant.ResetPasswordToken, uriString[1]);
                            await Current.MainPage.Navigation.PushModalAsync(new ResetPasswordPage());
                            Preferences.Remove(DFResources.EmailText);
                            Preferences.Remove(DFResources.PasswordText);
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            base.OnAppLinkRequestReceived(uri);
        }
        #endregion
    }
}
