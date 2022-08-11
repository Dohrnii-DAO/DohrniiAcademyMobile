using DohrniiFoundation.iOS.Services;
using DohrniiFoundation.IServices;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using UIKit;
using Xamarin.Forms;

namespace DohrniiFoundation.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        private App _app;
        // This method is invoked when the application has loaded and is ready to run. In this s
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            LoadApplication(new App());
            DependencyService.Register<IPlatformHelper, PlatformHelper>();
            return base.FinishedLaunching(app, options);
        }       
        /// <summary>
        /// This method is used to set the app orientation
        /// </summary>
        /// <param name="application"></param>
        /// <param name="forWindow"></param>
        /// <returns></returns>
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, [ObjCRuntime.Transient] UIWindow forWindow)
        {
            if (Xamarin.Forms.Application.Current == null || Xamarin.Forms.Application.Current.MainPage == null)
            {
              return UIInterfaceOrientationMask.Portrait;
            }
            else
            {
             return UIInterfaceOrientationMask.Portrait;               
            }
        }
    }
}
