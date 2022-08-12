using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.APIResponseModels.Lessons;
using DohrniiFoundation.Models.APIResponseModels.User;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.Lessons;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using PropertyChanged;
using DohrniiFoundation.Models.UserModels;
using DohrniiFoundation.Messages;

namespace DohrniiFoundation.ViewModels.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class LessonsViewModel : BaseViewModel
    {
        #region Private Properties
        private readonly ILessonService _lessonService;   
        private readonly ICacheService _cacheService;
        private readonly IMessenger _messenger;
        #endregion

        #region Public Properties
        public string LessonsBg { get; set; } = StringConstant.LessonsBg;
        public string Jellyfish { get; set; } = StringConstant.Jellyfish;
        public string LessonsDropup { get; set; } = StringConstant.LessonsDropup;
        public ObservableCollection<CategoryModel> ChaptersCategoryList { get; set; }
        public Models.Lessons.LessonInprogress CurrentLessonInProgress { get; set; }
        public int TotalXP { get; set; }
        public int TotalCryptoJelly { get; set; }
        public decimal TotalDHN { get; set; }
        public decimal ChapterProgress { get; set; }
        public string ChapterProgressPercentage { get; set; }
        public string LessonContinueName { get; set; }
        public string ContinueButtonText { get; set; }
        public string ShowContinueButton { get; set; }
        public bool ShowOnboarding { get; set; }
        public bool IsXP { get; set; }
        public bool IsJellyFish { get; set; }
        public bool IsDhn { get; set; }
        public string JellyAmount { get; set; }
        public bool ShowJellyAmount { get; set; }
        public ICommand ContinueChapterCommand { get; set; }
        public ICommand XPCommand { get; set; }
        public ICommand JellyfishCommand { get; set; }
        public ICommand DHNCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ConvertXPCommand { get; set; }
        public bool ShowLoader { get; set; }
        #endregion

        #region Constructor
        public LessonsViewModel()
        {
            try 
            {
                ContinueChapterCommand = new Command(ContinueChapterClick);
                XPCommand = new Command(XPClick);
                JellyfishCommand = new Command(JellyfishClick);
                DHNCommand = new Command(DHNCommandClick);
                CancelCommand = new Command(CancelCommandClick);
                ConvertXPCommand = new Command(ConvertXPClick);
                _lessonService = DependencyService.Get<ILessonService>(); //new LessonService();
                _cacheService = DependencyService.Get<ICacheService>();
                _messenger = DependencyService.Get<IMessenger>();
                ChaptersCategoryList = new ObservableCollection<CategoryModel>();
                Task.Run(async () =>
                {
                    await initData();
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void SubscribeEvent()
        {
            _messenger.Unsubscribe<UpdateLessonScreen>(this);
            _messenger.Subscribe<UpdateLessonScreen>(this, UpdateLessonScreen);
        }


        public async Task initData()
        {
            try
            {
                var categories = await _cacheService.GetCategories();
                if (categories != null)
                {
                    ChaptersCategoryList = new ObservableCollection<CategoryModel>(categories);
                    AppUtil.ChaptersCategorySelected = ChaptersCategoryList[0];
                    ChaptersCategoryList[0].IsSelected = true;

                    var userStatus = await _cacheService.GetUserStatus();
                    if (userStatus != null)
                    {
                        UpdateStatus(userStatus);
                    }
                    var progressData = await _cacheService.GetCategoryProgress(AppUtil.ChaptersCategorySelected.Id.ToString());
                    if (progressData != null)
                    {
                        UpdateProgress(progressData);
                    }
                }
                else
                {
                    IsLoading = true;
                    ShowLoader = true;
                }
                await UpdateFromServer();
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Crashes.TrackError(ex);
            }
        }

        protected async void UpdateLessonScreen(object sender, UpdateLessonScreen e)
        {
            try
            {
                var userStatusResponse = await _lessonService.GetUserStatus();
                if (userStatusResponse != null)
                {
                    UpdateStatus(userStatusResponse);
                    await _cacheService.SaveUserStatus(userStatusResponse);
                    var login = await _cacheService.GetCurrentUser();
                    login.User.TotalXp = userStatusResponse.TotalXP;
                    login.User.TotalDhn = userStatusResponse.TotalDHN;
                    login.User.TotalJelly = userStatusResponse.TotalCryptoJelly;
                    login.User.XpPerCryptojelly = userStatusResponse.XpPerCryptojelly;
                    await _cacheService.SaveCurrentUser(login);
                    AppUtil.CurrentUser = login.User;
                }

                var categoryProgress = await _lessonService.GetCategoryProgress(AppUtil.ChaptersCategorySelected.Id);
                if (categoryProgress != null)
                {
                    UpdateProgress(categoryProgress);
                    await _cacheService.SaveCategoryProgress(categoryProgress);
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Crashes.TrackError(ex);
            }
        }

        public async Task UpdateFromServer()
        {
            try
            {
                await GetChaptersCategories();
                await GetUserStatus();
                await GetChapterProgress();
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


        #endregion

        #region Methods
        public async Task GetChaptersCategories()
        {
            try
            {
                if (ShowLoader)
                {
                    IsLoading = true;
                }
                List<CategoryModel> categories = await _lessonService.GetCategories();
                if (categories.Count>0)
                {
                    ChaptersCategoryList = new ObservableCollection<CategoryModel>(categories);
                    AppUtil.ChaptersCategorySelected = ChaptersCategoryList[0];
                    ChaptersCategoryList[0].IsSelected = true;
                    await _cacheService.SaveCategories(categories);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(DFResources.OopsText, DFResources.SomethingWrongText, DFResources.OKText);
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

        private void UpdateProgress(CategoryProgress categoryProgress)
        {
            //REMARK: Bind the chapter progress depends on the classes
            decimal classCompleted = Convert.ToDecimal(categoryProgress.CompletedClass);
            decimal classesTotal = Convert.ToDecimal(categoryProgress.TotalClass);
            ChapterProgress = classCompleted / classesTotal;
            ChapterProgressPercentage = Math.Round(ChapterProgress * 100, MidpointRounding.ToEven).ToString() + "%";
        }

        private void UpdateStatus(UserStatus userStatus)
        {
            TotalXP = userStatus.TotalXP;
            TotalCryptoJelly = userStatus.TotalCryptoJelly;
            TotalDHN = Math.Round(userStatus.TotalDHN, 2);
            if (userStatus.LessonsInprogress != null)
            {
                foreach (var lessonsProgress in userStatus.LessonsInprogress)
                {
                    if (lessonsProgress.CategoryId == AppUtil.ChaptersCategorySelected.Id)
                    {
                        CurrentLessonInProgress = lessonsProgress;
                        LessonContinueName = lessonsProgress.LessonName;
                        if (lessonsProgress.IsNotStarted)
                        {
                            ContinueButtonText = DFResources.StartLessonText;
                        }
                        else
                        {
                            ContinueButtonText = DFResources.ContinueLessonText;
                        }
                    }
                }
            }
        }

        public async Task GetUserStatus()
        {
            try
            {
                if (ShowLoader)
                {
                    IsLoading = true;
                }
                var userStatusResponse = await _lessonService.GetUserStatus();
                if (userStatusResponse != null)
                {
                    UpdateStatus(userStatusResponse);
                    await _cacheService.SaveUserStatus(userStatusResponse);
                    var login = await _cacheService.GetCurrentUser();
                    login.User.TotalXp = userStatusResponse.TotalXP;
                    login.User.TotalDhn = userStatusResponse.TotalDHN;
                    login.User.TotalJelly = userStatusResponse.TotalCryptoJelly;
                    login.User.XpPerCryptojelly = userStatusResponse.XpPerCryptojelly;
                    await _cacheService.SaveCurrentUser(login);
                    AppUtil.CurrentUser = login.User;
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ResponseErrorPage());
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

        public async Task GetChapterProgress()
        {
            try
            {
                if (ShowLoader)
                {
                    IsLoading = true;
                }
                var categoryProgress = await _lessonService.GetCategoryProgress(AppUtil.ChaptersCategorySelected.Id);
                if (categoryProgress != null)
                {
                    UpdateProgress(categoryProgress);
                    await _cacheService.SaveCategoryProgress(categoryProgress);
                    ShowLoader = false;
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ResponseErrorPage());
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
        private async void ContinueChapterClick()
        {
            try
            {
                IsLoading = true;
                AppUtil.CurrentLessonInprogress = CurrentLessonInProgress;
                if (CurrentLessonInProgress.IsNotStarted)
                {
                    await _lessonService.StartLesson(new Models.StartRequestModel { Id = CurrentLessonInProgress.LessonId });
                }
                await Application.Current.MainPage.Navigation.PushModalAsync(new LessonChaptersPage());
                IsLoading = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private void XPClick()
        {
            try
            {
                AppUtil.SelectedLessonTypePoints = DFResources.XPText;
                IsXP = true;
                IsJellyFish = false;
                IsDhn = false;
                ShowOnboarding = true;
                //await Application.Current.MainPage.Navigation.PushPopupAsync(new LessonOnboardingPopUpPage());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void JellyfishClick()
        {
            try
            {
                AppUtil.SelectedLessonTypePoints = DFResources.CryptoJellyText;
                IsXP = false;
                IsJellyFish = true;
                IsDhn = false;
                ShowOnboarding = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DHNCommandClick()
        {
            try
            {
                AppUtil.SelectedLessonTypePoints = DFResources.DHNText; 
                IsXP = false;
                IsJellyFish = false;
                IsDhn = true;
                ShowOnboarding = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private void CancelCommandClick()
        {
            try
            {
                ShowOnboarding = false;
                ShowJellyAmount = false;
                JellyAmount = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ConvertXPClick()
        {
            try
            {
                if (ShowJellyAmount)
                {
                    if (!string.IsNullOrEmpty(JellyAmount))
                    {
                        int amount = 0;
                        try
                        {
                            amount = Convert.ToInt32(JellyAmount);
                        }
                        catch (Exception)
                        {
                            await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.InvalidJellyAmountText, DFResources.OKText);
                        }
                        if(amount > 0)
                        {
                            int requiredXP = amount * AppUtil.CurrentUser.XpPerCryptojelly;
                            if (requiredXP <= AppUtil.CurrentUser.TotalXp)
                            {
                                IsLoading = true;
                                var resp = await _lessonService.ConvertXptoJelly(new XPtoJellyModel { JellyAmount = amount});
                                if (resp != null)
                                {
                                    TotalXP = resp.TotalXp;
                                    TotalCryptoJelly = resp.TotalJelly;
                                    AppUtil.CurrentUser = resp;
                                    var login = await _cacheService.GetCurrentUser();
                                    login.User = resp;
                                    await _cacheService.SaveCurrentUser(login);
                                    ShowOnboarding = false;
                                    ShowJellyAmount = false;
                                    JellyAmount = string.Empty;
                                }
                                else
                                {
                                    await Application.Current.MainPage.Navigation.PushModalAsync(new ResponseErrorPage());
                                }
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, string.Format(DFResources.NotEnoughXPText, amount), DFResources.OKText);
                            }
                        }
                    }
                }
                else
                {
                    ShowJellyAmount = true;
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

        public Command CategoriesSelectedCommand
        {
            get
            {
                return new Command(async (param) =>
                {
                    try
                    {
                        if (ShowLoader)
                        {
                            IsLoading = true;
                        }
                        var selectedCategories = param as CategoryModel;
                        AppUtil.ChaptersCategorySelected = selectedCategories;
                        foreach (var chaptersCategories in ChaptersCategoryList)
                        {
                            if (chaptersCategories.Id == selectedCategories.Id)
                            {
                                chaptersCategories.IsSelected = true;
                            }
                            else
                            {
                                chaptersCategories.IsSelected = false;
                            }
                        }

                        var userStatus = await _cacheService.GetUserStatus();
                        if (userStatus != null)
                        {
                            UpdateStatus(userStatus);
                        }
                        var progressData = await _cacheService.GetCategoryProgress(AppUtil.ChaptersCategorySelected.Id.ToString());
                        if (progressData != null)
                        {
                            UpdateProgress(progressData);
                        }

                        if(userStatus == null || progressData == null)
                        {
                            IsLoading = true;
                        }

                        await GetChapterProgress();
                        await GetUserStatus();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }

        
        #endregion
    }
}
