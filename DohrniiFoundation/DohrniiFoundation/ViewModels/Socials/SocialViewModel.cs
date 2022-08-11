﻿using Acr.UserDialogs;
using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.MockData;
using DohrniiFoundation.Models.Socials;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views.Socials;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Socials
{
    /// <summary>
    /// View model to bind and implement the functionality of the social screen
    /// </summary>

    [AddINotifyPropertyChangedInterface]
    public class SocialViewModel : BaseViewModel
    {
        #region Private Properties
        private static IAPIService aPIService;
        private static IMockDataService mockDataService;
        private static ISocialService socialService;
        private static IUserService userService;

        #endregion

        #region Public Properties
        public ICommand TestingCommand { get; set; }
        public ICommand TodayCommand { get; set; }
        public ICommand WeeklyCommand { get; set; }
        public ICommand MonthlyCommand { get; set; }
        public ICommand AddFriendCommand { get; set; }
        public ICommand PendingCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand FilterUserCommand { get; set; }
        public ObservableCollection<LeaderBoard> LeaderBoards { get; set; }
        public List<AppUser> AllFriends { get; set; }
        public List<AppUser> Friends { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public AppUser AppUser { get; set; }
        public string TodayBgColor { get; set; }
        public string WeeklyBgColor { get; set; }
        public string MonthlyBgColor { get; set; }
        public bool ShowingAddFriendPopUp { get; set; }
        public string SearchQ { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the command
        /// </summary>
        public SocialViewModel()
        {
            try
            {
                TestingCommand = new Command(TestingClick);
                TodayCommand = new Command(TodayClick);
                WeeklyCommand = new Command(WeeklyClick);
                MonthlyCommand = new Command(MonthlyClick);
                AddFriendCommand = new Command(AddFriendClick);
                PendingCommand = new Command(PendingRequestClick);
                ShareCommand = new Command(ShareClick);
                FilterUserCommand = new Command(FilterTextChanged);
                aPIService = new APIServices();
                socialService = DependencyService.Get<ISocialService>(); //new SocialService();
                userService = DependencyService.Get<IUserService>(); //new UserService();
                mockDataService = new MockDataService();
                TodayBgColor = StringConstant.PrimaryBtnColor;
                WeeklyBgColor = StringConstant.Transparent;
                MonthlyBgColor = StringConstant.Transparent;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method is used handle the navigation when click on back
        /// </summary>
        private async void TestingClick()
        {
            try
            {
                IsLoading = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new TestingFontPage());
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void TodayClick()
        {
            try
            {
                SetBgColor();
                TodayBgColor = StringConstant.PrimaryBtnColor;
                await LoadData("Today");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void WeeklyClick()
        {
            try
            {
                SetBgColor();
                WeeklyBgColor = StringConstant.PrimaryBtnColor;
                await LoadData("Weekly");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void MonthlyClick()
        {
            try
            {
                SetBgColor();
                MonthlyBgColor = StringConstant.PrimaryBtnColor;
                await LoadData("Monthly");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void SetBgColor()
        {
            try
            {
                TodayBgColor = StringConstant.Transparent;
                WeeklyBgColor = StringConstant.Transparent;
                MonthlyBgColor = StringConstant.Transparent;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void AddFriendClick()
        {
            try
            {
                this.ShowingAddFriendPopUp = !this.ShowingAddFriendPopUp;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PendingRequestClick()
        {
            try
            {
                IsLoading = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new PendingRequestPage());
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ShareClick()
        {
            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = "https://dohrnii.io/academy",
                    Title = "Share Web Link"
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private void FilterTextChanged()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.SearchQ))
                {
                    var friends = this.AllFriends.Where(c => c.FirstName.ToLower().Contains(this.SearchQ.ToLower()) | c.LastName.ToLower().Contains(this.SearchQ.ToLower())).ToList();
                    Friends = new List<AppUser>(friends);
                }
                else
                {
                    Friends = new List<AppUser>(AllFriends);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task InitData()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                var leaders = await mockDataService.GetLeaderboard("Today");
                LeaderBoards = new ObservableCollection<LeaderBoard>(leaders);

                var friends = await mockDataService.GetFriends();
                AllFriends = new List<AppUser>(); //await socialService.GetFriends();
                Friends = new List<AppUser>(AllFriends);
                AppUtil.CurrentAppUser = await userService.GetAppUser();
                AppUsers = new List<AppUser>();// await userService.GetUsers();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }

        public async Task LoadData(string period)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                var leaders = await mockDataService.GetLeaderboard(period);
                LeaderBoards = new ObservableCollection<LeaderBoard>(leaders);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }

        public async Task ViewUserProfile(AppUser user)
        {
            try
            {
                IsLoading = true;
                await Application.Current.MainPage.Navigation.PushAsync(new UserProfilePage());
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }

        public async Task SendFriendRequest(AppUser user)
        {
            try
            {
                //var sendRequest = await UserDialogs.Instance.ConfirmAsync($"You are about to send a friend request to {user.FirstName}.\n Do you wish to continue?", "Friend Request", "Yes", "Cancel");
                //if (sendRequest)
                //{

                //}

                IsLoading = true;
                var result = await socialService.SendFriendRequests(user.Id, new FriendRequest());
                if (result != null)
                {
                    user.IsPendingRequest = true;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }
        #endregion
    }
}
