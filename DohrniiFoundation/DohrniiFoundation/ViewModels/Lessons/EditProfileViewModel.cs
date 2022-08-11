using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.APIRequestModel.User;
using DohrniiFoundation.Models.APIResponseModels.User;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views;
using Plugin.Permissions;
using Microsoft.AppCenter.Crashes;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using PropertyChanged;
using DohrniiFoundation.Models.UserModels;

namespace DohrniiFoundation.ViewModels.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class EditProfileViewModel : BaseViewModel
    {

        #region Public Properties
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WalletAddress { get; set; }
        public string ProfileImage { get; set; }
        public Color UserNameFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color FirstNameFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color LastNameFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public ICommand SaveCommand { get; set; }
        public ICommand CameraCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// This constructor is to initialize the objects
        /// </summary>

        public override Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }

        public EditProfileViewModel()
        {
            try
            {
                _userService = DependencyService.Get<IUserService>(); //new UserService();
                _cacheService = DependencyService.Get<ICacheService>(); //new CacheService();
                SaveCommand = new Command(SaveClick);
                CameraCommand = new Command(CameraClick);
                UserName = AppUtil.CurrentUser.UserName;
                FirstName = AppUtil.CurrentUser.FirstName;
                LastName = AppUtil.CurrentUser.LastName;
                WalletAddress = AppUtil.CurrentUser.WalletAddress;
                ProfileImage = AppUtil.CurrentUser.ProfileImage;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion

        #region Methods
        
        private async void SaveClick()
        {
            try
            {
                if (validateField())
                {
                    IsLoading = true;
                    var profile = new UpdateProfile
                    {
                        UserName = this.UserName,
                        FirstName = this.FirstName,
                        LastName = this.LastName,
                        WalletAddress = this.WalletAddress
                    };
                    var user = await _userService.UpdateProfile(profile);
                    if(user != null)
                    {
                        AppUtil.CurrentUser = user;
                        var login = await _cacheService.GetCurrentUser();
                        login.User = user;
                        await _cacheService.SaveCurrentUser(login);
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(DFResources.OopsText, DFResources.SomethingWrongText, DFResources.OKText);
                    }
                }
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
        
        private async void CameraClick()
        {
            try
            {
                string action = await Application.Current.MainPage.DisplayActionSheet(DFResources.SelectOptionText, DFResources.CancelText, null, DFResources.CameraText, DFResources.GalleryText);
                if (action == DFResources.CameraText)
                {
                    //await TakePhoto();
                }
                else if (action == DFResources.GalleryText)
                {
                    //await SelectPhoto();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }


        private bool validateField()
        {
            bool IsValidUserName = Regex.IsMatch(UserName, StringConstant.UserNameRegex);
            if (IsValidUserName)
            {
                if (!string.IsNullOrEmpty(this.FirstName))
                {
                    if (!string.IsNullOrEmpty(this.LastName))
                    {
                        return true;
                    }
                    else
                        LastNameFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                }
                else
                    FirstNameFrameBorderColor = (Color)Application.Current.Resources["RedText"];
            }
            else
                UserNameFrameBorderColor = (Color)Application.Current.Resources["RedText"];
            return false;

        }

        public Command UserNameTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (string.IsNullOrEmpty(UserName))
                    {
                        UserNameFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                    }
                    else
                    {
                        if (UserName.Contains(" "))
                        {
                            UserName = UserName.Replace(" ", string.Empty);
                        }
                        else
                        {
                            bool IsValid = Regex.IsMatch(UserName, StringConstant.UserNameRegex);
                            UserNameFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                        }
                    }
                });
            }
        }

        public Command FirstNameTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {
                    FirstNameFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                });
            }
        }
        public Command LastNameTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {
                    LastNameFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                });
            }
        }

        #endregion
    }
}
