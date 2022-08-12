using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.APIResponseModels;
using DohrniiFoundation.Models.APIResponseModels.User;
using DohrniiFoundation.Models.More;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.ViewModels.Lessons;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.More;
using DohrniiFoundation.Views.User;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.More
{
    [AddINotifyPropertyChangedInterface]
    public class ProfileMoreViewModel : BaseViewModel
    {

        #region Public Properties
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        public ObservableCollection<ProfileMoreModel> ProfileMoreList { get; set; }
        public Models.UserModels.User User { get; set; }
        public bool ShowCS { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand HideCSCommand { get; set; }
        #endregion

        #region Constructor
        public ProfileMoreViewModel()
        {
            try
            {
                _userService = DependencyService.Get<IUserService>(); //new UserService();
                _cacheService = DependencyService.Get<ICacheService>(); //new CacheService();
                SendCommand = new Command(SendClick);
                HideCSCommand = new Command(HideCSClick);
                GetProfileList();
                
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion

        #region Methods

        private async void SendClick()
        {

        }
        private void HideCSClick()
        {
            this.ShowCS = false;
        }

        public async Task GetProfileDetails()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                var login = await _cacheService.GetCurrentUser();
                this.User = login.User;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                IsLoading = false;
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }
        private void GetProfileList()
        {
            try
            {
                ProfileMoreList = new ObservableCollection<ProfileMoreModel>()
                {
                    new ProfileMoreModel()
                    {
                        Title = DFResources.ChangePasswordText,
                        TitleImage = StringConstant.LockIcon,
                        IsNextVisible = true,
                    },
                    new ProfileMoreModel()
                    {
                        Title = DFResources.BiometricDetailsText,
                        TitleImage = StringConstant.FingerPrint,
                        IsNextVisible = true,
                    },
                    new ProfileMoreModel()
                    {
                        Title = DFResources.FAQText,
                        TitleImage = StringConstant.FaqIcon,
                        IsNextVisible = true,
                    },
                    new ProfileMoreModel()
                    {
                        Title = DFResources.CustomerSupportText,
                        TitleImage = StringConstant.HeadphonesIcon,
                        IsNextVisible = true,
                    },
                    new ProfileMoreModel()
                    {
                        Title = DFResources.PrivacyPolicyText,
                        TitleImage = StringConstant.PolicyIcon,
                        IsNextVisible = true,
                    },
                    new ProfileMoreModel()
                    {
                        Title = DFResources.LogoutText,
                        TitleImage = StringConstant.LogoutIcon,
                        IsNextVisible = false,
                    },
                };
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        /// <summary>
        /// This method is to select the profile list item from the list
        /// </summary>
        /// <param name="selectedItem"></param>
        public async void GetSelectedItem(ProfileMoreModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    if (selectedItem.Title == DFResources.ChangePasswordText)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage());
                    }
                    else if (selectedItem.Title == DFResources.CustomerSupportText)
                    {
                        this.ShowCS = true;
                    }
                    else if (selectedItem.Title == DFResources.LogoutText)
                    {
                        bool result = await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.LogoutAlertText, DFResources.YesText, DFResources.NoText);
                        if (result)
                        {
                            await LogoutApp();
                        }
                    }
                    else if (selectedItem.Title == DFResources.PrivacyPolicyText)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new PrivacyPolicyPage());
                    }
                    else if (selectedItem.Title == DFResources.FAQText)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new FAQPage());
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        /// <summary>
        ///  Method to logout from app and API implementation
        /// </summary>
        /// <returns></returns>
        public async Task LogoutApp()
        {
            try
            {
                Preferences.Remove("accessToken");
                await _cacheService.Logout();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// This Command is used for the edit profile button.
        /// </summary>
        public Command CommandEditProfile
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new EditProfilePage());
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Source);
                    }
                });
            }
        }
        #endregion
    }
}
