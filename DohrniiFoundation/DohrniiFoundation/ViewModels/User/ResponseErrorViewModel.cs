using DohrniiFoundation.Helpers;
using DohrniiFoundation.Views.Lessons;
using DohrniiFoundation.Views.More;
using DohrniiFoundation.Views.Socials;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.User
{
    [AddINotifyPropertyChangedInterface]
    public class ResponseErrorViewModel : BaseViewModel
    {
        public ICommand TryAgainCommand { get; set; }

        public ResponseErrorViewModel()
        {
            TryAgainCommand = new Command(TryAgainClick);
        }

        private void TryAgainClick()
        {
            try
            {
                int tabId = Preferences.Get(StringConstant.TabIdKey, 0);
                switch (tabId)
                {
                    case 0:
                    case 1:
                        Preferences.Set(StringConstant.TabIdKey, 1);
                        Application.Current.MainPage = new NavigationPage(new SocialPage());
                        break;
                    case 2:
                        Preferences.Set(StringConstant.TabIdKey, 2);
                        Application.Current.MainPage = new NavigationPage(new LessonsPage());
                        break;
                    case 3:
                        Preferences.Set(StringConstant.TabIdKey, 2);
                        Application.Current.MainPage = new NavigationPage(new LessonsPage());
                        break;
                    case 4:
                        Preferences.Set(StringConstant.TabIdKey, 4);
                        Application.Current.MainPage = new NavigationPage(new ProfileMorePage());
                        break;
                    default:
                        Preferences.Set(StringConstant.TabIdKey, 2);
                        Application.Current.MainPage = new NavigationPage(new LessonsPage());
                        break;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
