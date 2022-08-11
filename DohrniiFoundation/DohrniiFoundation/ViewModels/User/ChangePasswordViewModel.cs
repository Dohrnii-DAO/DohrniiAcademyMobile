using System;
using DohrniiFoundation.Helpers;
using Microsoft.AppCenter.Crashes;
using System.Windows.Input;
using Xamarin.Forms;
using DohrniiFoundation.Resources;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Services;
using DohrniiFoundation.Models.APIResponseModels;
using DohrniiFoundation.Models.APIRequestModel.User;
using DohrniiFoundation.Views;
using System.Text.RegularExpressions;
using PropertyChanged;

namespace DohrniiFoundation.ViewModels.User
{
    [AddINotifyPropertyChangedInterface]
    public class ChangePasswordViewModel : BaseViewModel
    {

        #region Public Properties
        private readonly IUserService _userService;
        public string Lock { get; set; } = StringConstant.Lock;
        public string BackArrow { get; set; } = StringConstant.BackArrow;
        public string PasswordIcon { get; set; } = StringConstant.PasswordIcon;
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public Color OldPasswordFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color NewPasswordFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color ConfirmPasswordFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Constructor   
        public ChangePasswordViewModel()
        {
            try
            {
                SaveCommand = new Command(SaveClick);
                _userService = DependencyService.Get<IUserService>(); //new UserService();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion

        #region Methods

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
        private bool PasswordValidation()
        {
            bool result = false;
            try
            {
                if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
                {
                    Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.AllMandatoryText, DFResources.OKText);
                }
                else if ((!string.IsNullOrEmpty(OldPassword) && !Utilities.IsValidPassword(OldPassword)) || (!string.IsNullOrEmpty(NewPassword) && !Utilities.IsValidPassword(NewPassword)) || !string.IsNullOrEmpty(ConfirmPassword) && !Utilities.IsValidPassword(ConfirmPassword))
                {
                    Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.PleaseEnterValidPassword, DFResources.OKText);
                }
                else if (!string.IsNullOrEmpty(NewPassword) && NewPassword != ConfirmPassword)
                {
                    Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.NewAndConfirmPasswordSameText, DFResources.OKText);
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                result = false;
            }

            return result;
        }
        /// <summary>
        /// Method to save the changed password using change password API
        /// </summary>
        private async void SaveClick()
        {
            try
            {
                if (PasswordValidation())
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsLoading = true;
                    });
                    ChangePasswordRequestModel changePasswordRequestModel = new ChangePasswordRequestModel()
                    {
                        OldPassword = OldPassword,
                        NewPassword = NewPassword
                    };
                    var result = await _userService.ChangePassword(changePasswordRequestModel);
                    if (result != null)
                    {
                        await Application.Current.MainPage.DisplayAlert(DFResources.SuccessText, result.Message, DFResources.OKText);
                        await Application.Current.MainPage.Navigation.PopAsync();
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
        /// <summary>
        /// This command to set border color of old password and check validation of old password
        /// </summary>
        public Command OldPasswordTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (string.IsNullOrEmpty(OldPassword))
                    {
                        OldPasswordFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                    }
                    else
                    {
                        if (OldPassword.Contains(" "))
                        {
                            OldPassword = OldPassword.Replace(" ", string.Empty);
                        }
                        else
                        {
                            bool IsValid = Regex.IsMatch(OldPassword, StringConstant.passwordRegex);
                            OldPasswordFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                        }
                    }
                });
            }
        }
        /// <summary>
        /// This command to set border color of new password and check validation of new password 
        /// </summary>
        public Command NewPasswordTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (string.IsNullOrEmpty(NewPassword))
                    {
                        NewPasswordFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                    }
                    else
                    {
                        if (NewPassword.Contains(" "))
                        {
                            NewPassword = NewPassword.Replace(" ", string.Empty);
                        }
                        else
                        {
                            bool IsValid = Regex.IsMatch(NewPassword, StringConstant.passwordRegex);
                            NewPasswordFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                        }
                    }
                });
            }
        }
        /// <summary>
        /// This command is to set the border color of the confirm password text entry
        /// </summary>
        public Command ConfirmPasswordTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (!string.IsNullOrEmpty(NewPassword))
                    {
                        if (string.IsNullOrEmpty(ConfirmPassword))
                        {
                            ConfirmPasswordFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                        }
                        else
                        {
                            if (ConfirmPassword.Contains(" "))
                            {
                                ConfirmPassword = ConfirmPassword.Replace(" ", string.Empty);
                            }
                            else
                            {
                                bool IsValid = NewPassword.Equals(ConfirmPassword);
                                ConfirmPasswordFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                            }
                        }
                    }
                    else
                    {
                        ConfirmPassword = string.Empty;
                        Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.EnterNewPasswordFirstText, DFResources.OKText);
                    }
                });
            }
        }
        #endregion
    }
}
