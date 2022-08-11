using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.UserModels;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.Lessons;
using DohrniiFoundation.Views.User;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.User
{
    public class RegisterViewModel: BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        public ICommand SubmitCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand SignUpCommand { get; set; }
        public string Google { get; set; } = StringConstant.Google;
        public string Fb { get; set; } = StringConstant.Fb;
        public string Apple { get; set; } = StringConstant.Apple;
        public string EmailIcon { get; set; } = StringConstant.EmailIcon;
        public string PasswordIcon { get; set; } = StringConstant.PasswordIcon;
        public string UserIcon { get; set; } = StringConstant.UserIcon;

        public string Email { get; set; }
        public bool IsWelcome { get; set; } = true;
        public bool IsNext { get; set; }
        public bool IsSignUp { get; set; }
        public bool IsSuccessful { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool ShowPassword { get; set; }
        public string UserName { get; set; }

        public Color EmailFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color UserNameFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color PasswordFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color ConfirmPasswordFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color FirstNameFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color LastNameFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];
        public Color CodeFrameBorderColor { get; set; } = (Color)Application.Current.Resources["LessonXPFirstColor"];

        public RegisterViewModel()
        {
            _userService = DependencyService.Get<IUserService>(); //new UserService();
            _cacheService = DependencyService.Get<ICacheService>(); //new CacheService();
            SubmitCommand = new Command(SubmitClick);
            NextCommand = new Command(NextClick);
            SignUpCommand = new Command(SignUpClick);
        }

        private async void SubmitClick()
        {
            try
            {
                bool IsValid = Regex.IsMatch(Email ?? "", StringConstant.EmailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                if (IsValid)
                {
                    bool IsValidPassword = Regex.IsMatch(Password ?? "", StringConstant.passwordRegex);
                    if (IsValidPassword)
                    {
                        bool IsMatched = Password.Equals(ConfirmPassword);
                        if (IsMatched)
                        {
                            IsLoading = true;
                            var obj = new RegisterModel
                            {
                                Email = this.Email,
                                Device = $"{DeviceInfo.Manufacturer} {DeviceInfo.Model}, OS Version: {DeviceInfo.VersionString}"
                            };
                            var result = await _userService.Register(obj);
                            if (result != null)
                            {
                                IsWelcome = false;
                                IsNext = true;
                            }
                        }
                        else
                            ConfirmPasswordFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                    }
                    else
                        PasswordFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                    
                }
                else
                    EmailFrameBorderColor = (Color)Application.Current.Resources["RedText"];
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private void NextClick()
        {
            try
            {
                IsWelcome = false;
                IsNext = false;
                IsSignUp = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }
        private async void SignUpClick()
        {
            try
            {
                if (validateField())
                {
                    IsLoading = true;
                    var obj = new UserValidationModel
                    {
                        Email = this.Email,
                        Password = this.Password,
                        FirstName = this.FirstName,
                        LastName = this.LastName,
                        Code = this.VerificationCode,
                        UserName = this.UserName
                    };
                    var result = await _userService.ValidateUser(obj);
                    if (result != null)
                    {
                        if (string.IsNullOrEmpty(result.AccessToken))
                        {
                            //await Application.Current.MainPage.Navigation.PopModalAsync();
                            IsWelcome = false;
                            IsNext = false;
                            IsSignUp = false;
                            IsSuccessful = true;
                        }
                        else
                        {
                            Preferences.Set("accessToken", result.AccessToken);
                            AppUtil.CurrentUser = result.User;
                            await _cacheService.SaveCurrentUser(result);
                            await Application.Current.MainPage.Navigation.PopModalAsync();
                            Application.Current.MainPage = new NavigationPage(new LessonsPage());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsLoading = false;
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
                        bool IsValidPassword = Regex.IsMatch(Password ?? "", StringConstant.passwordRegex);
                        if (IsValidPassword)
                        {
                            bool IsMatched = Password.Equals(ConfirmPassword);
                            if (IsMatched)
                            {
                                if (!string.IsNullOrEmpty(VerificationCode))
                                {
                                    return true;
                                }
                                else
                                    CodeFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                            }
                            else
                                ConfirmPasswordFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                        }
                        else
                            PasswordFrameBorderColor = (Color)Application.Current.Resources["RedText"];
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

        /// <summary>
        /// This command to set border color of email and check validation of email 
        /// </summary>
        public Command EmailTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (string.IsNullOrEmpty(Email))
                    {
                        EmailFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                    }
                    else
                    {
                        bool IsValid = Regex.IsMatch(Email, StringConstant.EmailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                        EmailFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                    }
                });
            }
        }

        /// <summary>
        /// This command to set border color of user name and check validation of user name 
        /// </summary>
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
                            if (IsValid)
                            {
                                UserNameFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                            }
                            else
                            {
                                UserNameFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                            }
                        }
                    }
                });
            }
        }
        /// <summary>
        /// This command to set border color of password and check validation of password 
        /// </summary>
        public Command PasswordTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (string.IsNullOrEmpty(Password))
                    {
                        PasswordFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                    }
                    else
                    {
                        if (Password.Contains(" "))
                        {
                            Password = Password.Replace(" ", string.Empty);
                        }
                        else
                        {
                            bool IsValid = Regex.IsMatch(Password, StringConstant.passwordRegex);
                            PasswordFrameBorderColor = IsValid ? (Color)Application.Current.Resources["LessonXPFirstColor"] : (Color)Application.Current.Resources["RedText"];
                        }
                    }
                });
            }
        }
        /// <summary>
        /// This command to set border color of confirm password and check validation of confirm password 
        /// </summary>
        public Command ConfirmPasswordTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (!string.IsNullOrEmpty(Password))
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
                                bool IsNewValid = Regex.IsMatch(ConfirmPassword, StringConstant.passwordRegex);
                                if (IsNewValid)
                                {
                                    bool IsValid = Password.Equals(ConfirmPassword);
                                    if (IsValid)
                                    {
                                        ConfirmPasswordFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                                    }
                                    else
                                    {
                                        ConfirmPasswordFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                                    }
                                }
                                else
                                {
                                    ConfirmPasswordFrameBorderColor = (Color)Application.Current.Resources["RedText"];
                                }
                            }
                        }
                    }
                    else
                    {
                        ConfirmPassword = string.Empty;
                        Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.EnterPasswordFirstText, DFResources.OKText);
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
        public Command CodeTextChangedCommand
        {
            get
            {
                return new Command(() =>
                {
                    CodeFrameBorderColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                });
            }
        }
    }
}
