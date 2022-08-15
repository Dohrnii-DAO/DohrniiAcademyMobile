using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Messages;
using DohrniiFoundation.Models.APIRequestModel.Lessons;
using DohrniiFoundation.Models.APIResponseModels;
using DohrniiFoundation.Models.APIResponseModels.Lessons;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.Lessons;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class LessonChaptersViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IAPIService aPIService;
        private readonly ILessonService _lessonService;
        private readonly IAppState _appState;
        private readonly ICacheService _cacheService;
        private readonly IMessenger _messenger;

        #endregion

        #region Public Properties
        public string LessonsDropdown { get; set; } = StringConstant.LessonsDropdown;

        public ChapterModel ChapterDetail { get; set; }
        public LessonModel SelectedLesson { get; set; }
        public ObservableCollection<ChaptersModel> ChaptersList { get; set; }
        public bool ShowClasses { get; set; }
        public ObservableCollection<ProgressBarModel> ProgressBarList { get; set; }
        public ObservableCollection<ClassesProgressBarModel> ClassesProgressBarList { get; set; }
        public ObservableCollection<ClassModel> ClasseslList { get; set; }
        public bool ChaptersListVisible { get; set; }
        public bool ClassesPopUpVisible { get; set; }
        public string SelectedLessonName { get; set; }
        public string LessonTitle { get; set; }
        public int TotalClasses { get; set; }
        public int SelectedClassNumber { get; set; }
        public string XPTokensCollected { get; set; }
        public string TotalXPCollected { get; set; }
        public decimal XPTotalProgress { get; set; }
        public bool ShowUnlockQuiz { get; set; }
        public bool ShowUnlockButton { get; set; }
        public Color LastProgressFrameColor { get; set; } = (Color)Application.Current.Resources["LessonSegmentColor"];
        public string ClickhereText { get; set; }
        public int Position { get; set; }   
        public ClassModel ClassCurrentItem { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ContinueLessonCommand { get; set; }
        public ICommand ShowUnlockCommand { get; set; }
        public ICommand CloseUnlockCommand { get; set; }
        public ICommand StartQuizCommand { get; set; }
        public ICommand UnlockQuizCommand { get; set; }
        public override Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Application.Current.MainPage.Navigation.PopModalAsync();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }
        #endregion

        #region Constructor
        public LessonChaptersViewModel()
        {
            try
            {
                _lessonService = DependencyService.Get<ILessonService>(); //new LessonService();
                _appState = DependencyService.Get<IAppState>();
                _cacheService = DependencyService.Get<ICacheService>();
                _messenger = DependencyService.Get<IMessenger>();
                CancelCommand = new Command(CancelClick);
                ContinueLessonCommand = new Command(ContinueClick);
                ShowUnlockCommand = new Command(ShowUnlockClick);
                CloseUnlockCommand = new Command(ShowUnlockClick);
                StartQuizCommand = new Command(StartQuizClick);
                UnlockQuizCommand = new Command(UnlockQuizClick);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion

        #region Methods      

        public void InitData()
        {
            _messenger.Unsubscribe<UpdateChapterDetailScreen>(this);
            _messenger.Subscribe<UpdateChapterDetailScreen>(this, UpdateChapterDetailScreen);
            GetChapterDetails(AppUtil.CurrentLessonInprogress.ChapterId);
        }

        protected async void UpdateChapterDetailScreen(object sender, UpdateChapterDetailScreen e)
        {
            try
            {
                var chapterResponse = await _lessonService.GetChapter(AppUtil.CurrentLessonInprogress.ChapterId);
                if (chapterResponse != null)
                {
                    this.ChapterDetail = chapterResponse;
                    _appState.ChapterDetail = chapterResponse;
                    await _cacheService.SaveChapterDetail(chapterResponse);
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Crashes.TrackError(ex);
            }
            
        }

        public async void GetChapterDetails(int chapterId)
        {
            try
            {
                IsLoading = true;
                var cacheData = await _cacheService.GetChapterDetail(chapterId.ToString());
                if (cacheData != null)
                {
                    this.ChapterDetail = cacheData;
                    this.ClickhereText = string.Format(DFResources.ClickHereToUlockChapteQuizText, ChapterDetail.RequiredJelly);
                    _appState.ChapterDetail = cacheData;
                    IsLoading = false;
                    ShowUnlockButton = true;
                }
                var chapterResponse = await _lessonService.GetChapter(chapterId);
                if (chapterResponse != null)
                {
                    this.ChapterDetail = chapterResponse;
                    this.ClickhereText = string.Format(DFResources.ClickHereToUlockChapteQuizText, ChapterDetail.RequiredJelly);
                    _appState.ChapterDetail = chapterResponse;
                    await _cacheService.SaveChapterDetail(chapterResponse);
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
                ShowUnlockButton = true;
            }
        }

        private void CancelClick()
        {
            try
            {
                ShowClasses = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private async void ShowUnlockClick()
        {
            try
            {
                if (ChapterDetail.IsQuizDone)
                {
                    await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.QuizAlreadyTakenText, DFResources.OKText);
                }
                else if (ChapterDetail.IsQuizUnlocked)
                {
                    AppUtil.SelectedChapter = this.ChapterDetail;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ChapterQuestionPage());
                }
                else
                {
                    ShowUnlockQuiz = !ShowUnlockQuiz;
                }
            }
            catch (Exception ex)
            {
                ShowUnlockQuiz = false;
                Crashes.TrackError(ex);
            }
        }
        private async void StartQuizClick()
        {
            try
            {
                ShowUnlockQuiz = false;
                AppUtil.SelectedChapter = this.ChapterDetail;
                await Application.Current.MainPage.Navigation.PushModalAsync(new ChapterQuestionPage());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private async void UnlockQuizClick()
        {
            try
            {
                IsLoading = true;
                if(AppUtil.CurrentUser.TotalJelly >= this.ChapterDetail.RequiredJelly)
                {
                    var resp = await _lessonService.UnlockChapterQuiz(new UnlockQuizModel { ChapterId = ChapterDetail.Id });
                    if (resp != null)
                    {
                        this.ChapterDetail.IsQuizUnlocked = true;
                        _appState.ChapterDetail = this.ChapterDetail;
                        await _cacheService.SaveChapterDetail(this.ChapterDetail);
                        AppUtil.CurrentUser = resp;
                        var login = await _cacheService.GetCurrentUser();
                        login.User = resp;
                        await _cacheService.SaveCurrentUser(login);
                        _messenger.Send(new UpdateLessonScreen());
                    }
                    else
                    {
                        await Application.Current.MainPage.Navigation.PushModalAsync(new ResponseErrorPage());
                    }
                }
                else {
                    await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.NotEnoughJellyText, DFResources.OKText);
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
        private async void ContinueClick()
        {
            try
            {
                if (SelectedLesson.Classes.Count > 0)
                {
                    AppUtil.SelectedClass = SelectedLesson.Classes.FirstOrDefault(c => c.IsStarted == true && c.IsCompleted == false);
                    if (AppUtil.SelectedClass == null)
                    {
                        AppUtil.SelectedClass = SelectedLesson.Classes[0];
                    }
                    this.ShowClasses = false;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new LessonsDetailPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(DFResources.NoticeText, DFResources.NoAvailableClassText, DFResources.OKText);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        public Command LessonSelectedCommand
        {
            get
            {
                return new Command(async (param) =>
                {
                    try
                    {
                        var selectedLesson = param as LessonModel;
                        if (!selectedLesson.IsStarted)
                        {
                            await Application.Current.MainPage.DisplayAlert(DFResources.LessonLockedAlertText, DFResources.LessonLockedMsgText, DFResources.OKText);
                        }
                        else
                        {
                            this.SelectedLesson = selectedLesson;
                            AppUtil.SelectedLesson = selectedLesson;
                            this.ShowClasses = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }
        public Command PreviousCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (ClasseslList.Count > 1)
                        {
                            if (Position > 0)
                            {
                                var nextPosition = --Position;
                                Position = nextPosition;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }
        public Command NextCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (ClasseslList.Count > 1)
                        {
                            if (Position != ClasseslList.Count - 1)
                            {
                                var nextPosition = ++Position;
                                Position = nextPosition;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }





















        /// <summary>
        /// This method is to integrate the all chapters and lessons API and handle the functionality
        /// </summary>
        public async void GetAllChaptersAndLessons()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                ChaptersList = new ObservableCollection<ChaptersModel>();
                int chapterNumber = 0;
                int lessonNumber = 0;
                ChaptersCategoryRequestModel chaptersCategoryRequestModel = new ChaptersCategoryRequestModel()
                {
                    SelectedCategory = AppUtil.ChaptersCategorySelected.Id,
                };
                LessonsResponseModel lessonsResponseModel = await aPIService.GetChaptersLessonsCategoryService(chaptersCategoryRequestModel);
                if (lessonsResponseModel != null)
                {
                    if (lessonsResponseModel.StatusCode == 200 && lessonsResponseModel.Status)
                    {
                        if (lessonsResponseModel.AllChapters != null)
                        {
                            foreach (ChaptersModel lessonsData in lessonsResponseModel.AllChapters)
                            {
                                chapterNumber++;
                                lessonNumber = 0;
                                if (lessonsData.LessonDataList != null)
                                {
                                    foreach (Models.Lessons.Lessons lessonsItem in lessonsData.LessonDataList)
                                    {
                                        lessonNumber++;
                                        lessonsItem.LessonsNumber = lessonNumber;
                                        //REMARK: condition to bind lessons progress bar 
                                        if (lessonsItem.Status == StringConstant.ClassUnlocked)
                                        {
                                            lessonsItem.ProgressBarList = new ObservableCollection<ProgressBarModel>();
                                            ProgressBarList = new ObservableCollection<ProgressBarModel>();
                                            ProgressBarModel lessonsProgressBarList = new ProgressBarModel();
                                            lessonsItem.LessonsCompletedClass = lessonsItem.CompleteClass;

                                            //REMARK: handle the background color of progress if class is completed
                                            for (int i = 1; i <= lessonsItem.TotalLinkClass; i++)
                                            {
                                                ProgressBarModel lessonsProgressBarList1 = new ProgressBarModel();
                                                if (Convert.ToInt32(lessonsItem.LessonsCompletedClass) != 0)
                                                {
                                                    lessonsProgressBarList1.LessonsName = lessonsData.Name;
                                                    lessonsProgressBarList1.StackWidth = Application.Current.MainPage.Width;
                                                    lessonsProgressBarList1.BorderCircleFrameColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                                                    lessonsProgressBarList1.BoxviewFrameColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                                                    lessonsProgressBarList1.FrameCircleBgColor = (Color)Application.Current.Resources["WhiteText"];
                                                    lessonsItem.LessonsCompletedClass = (Convert.ToInt32(lessonsItem.LessonsCompletedClass) - 1).ToString();
                                                }
                                                else
                                                {
                                                    lessonsProgressBarList1.LessonsName = lessonsData.Name;
                                                    lessonsProgressBarList1.StackWidth = Application.Current.MainPage.Width;
                                                    lessonsProgressBarList1.BorderCircleFrameColor = (Color)Application.Current.Resources["LessonSegmentColor"];
                                                    lessonsProgressBarList1.BoxviewFrameColor = (Color)Application.Current.Resources["LessonSegmentColor"];
                                                    lessonsProgressBarList1.FrameCircleBgColor = (Color)Application.Current.Resources["WhiteText"];
                                                }
                                                lessonsItem.ProgressBarList.Add(lessonsProgressBarList1);
                                            }
                                            //REMARK: classes progress in percentage in lessons
                                            if (lessonsItem.CompleteClass != "0")
                                            {
                                                decimal classCompleted = Convert.ToDecimal(lessonsItem.CompleteClass);
                                                decimal classesTotal = Convert.ToDecimal(lessonsItem.TotalLinkClass);
                                                decimal classProgress = classCompleted / classesTotal;
                                                lessonsItem.ClassPercentage = Math.Round(classProgress * 100, MidpointRounding.ToEven).ToString();
                                            }
                                            else
                                            {
                                                lessonsItem.ClassPercentage = "0";
                                            }
                                        }

                                        if (Utilities.SelectedLesson != null)
                                        {
                                            if (Utilities.SelectedLesson.Id == lessonsItem.Id)
                                            {
                                                if (lessonsData.Id == Utilities.SelectedLesson.ChapterId)
                                                {
                                                    Utilities.ClassXPTokensCollected = lessonsItem.EarnXP;
                                                    XPTokensCollected = Utilities.ClassXPTokensCollected;
                                                }
                                            }
                                        }
                                    }
                                    //REMARK: Get the last status of the lesson of the completed chapter
                                    string lastLessonStatus = lessonsData.LessonDataList[lessonsData.LessonDataList.Count - 1].Status;
                                    //REMARK: Bind the chapters quiz status depends on the value from API
                                    if (lessonsData.ChapterQuizStatus == StringConstant.ClassUnlocked)
                                    {
                                        lessonsData.QuizStatusName = StringConstant.Unlocked;
                                        lessonsData.IsQuizCompleted = false;
                                        lessonsData.IsQuizUnlock = true;
                                        lessonsData.IsQuizLocked = false;
                                        lessonsData.CryptoJelly = lessonsData.CryptoJelly;
                                    }
                                    else if (lessonsData.ChapterQuizStatus == StringConstant.ClassPassed)
                                    {
                                        lessonsData.QuizStatusName = StringConstant.Complete;
                                        lessonsData.IsQuizCompleted = true;
                                        lessonsData.IsQuizUnlock = false;
                                        lessonsData.IsQuizLocked = false;
                                        lessonsData.CryptoJelly = 0;
                                    }
                                    else if (lessonsData.ChapterQuizStatus == StringConstant.ClassLocked)
                                    {
                                        if (lastLessonStatus != "1")
                                        {
                                            lessonsData.QuizStatusName = StringConstant.Locked;
                                            lessonsData.IsQuizCompleted = false;
                                            lessonsData.IsQuizUnlock = false;
                                            lessonsData.IsQuizLocked = true;
                                            lessonsData.CryptoJelly = lessonsData.CryptoJelly;
                                        }
                                        else
                                        {
                                            if (lessonsData.CryptoJelly <= Utilities.UserTotalCryptoJelly)
                                            {
                                                ResponseModel unlockQuizResponse = await UnlockChapterQuiz(lessonsData);
                                                if (unlockQuizResponse.Status)
                                                {
                                                    lessonsData.ChapterQuizStatus = StringConstant.ClassUnlocked;
                                                    lessonsData.IsQuizCompleted = false;
                                                    lessonsData.IsQuizUnlock = true;
                                                    lessonsData.IsQuizLocked = false;
                                                    lessonsData.CryptoJelly = lessonsData.CryptoJelly;
                                                }
                                            }
                                            else
                                            {
                                                lessonsData.IsQuizCompleted = false;
                                                lessonsData.IsQuizUnlock = false;
                                                lessonsData.IsQuizLocked = true;
                                            }
                                        }
                                    }
                                    decimal totalClasses = Convert.ToDecimal(lessonsData.TotalClasses);
                                    decimal completedClasses = Convert.ToDecimal(lessonsData.CompleteClasses);
                                    lessonsData.ChaptersProgressPercentage = completedClasses / totalClasses;
                                    ChaptersList.Add(new ChaptersModel() { Id = lessonsData.Id, Name = lessonsData.Name, Description = lessonsData.Description, CategoryId = lessonsData.CategoryId, CategoryName = lessonsData.CategoryName, ChapterQuizStatus = lessonsData.ChapterQuizStatus, QuizStatusName = lessonsData.QuizStatusName, CryptoJelly = lessonsData.CryptoJelly, IllustrationImage = lessonsData.IllustrationImage, CompleteClasses = lessonsData.CompleteClasses, TotalClasses = lessonsData.TotalClasses, ChaptersProgressPercentage = lessonsData.ChaptersProgressPercentage, ChapterNumber = chapterNumber, IsQuizCompleted = lessonsData.IsQuizCompleted, IsQuizUnlock = lessonsData.IsQuizUnlock, IsQuizLocked = lessonsData.IsQuizLocked, QuizTimer = lessonsData.QuizTimer, LessonDataList = lessonsData.LessonDataList });
                                    Utilities.ChaptersListData = ChaptersList;
                                }
                            }
                            foreach (ChaptersModel chapters in ChaptersList)
                            {
                                foreach (Models.Lessons.Lessons lessons in chapters.LessonDataList)
                                {
                                    if (lessons.Status == StringConstant.LessonInprogress)
                                    {
                                        lessons.IsCompleted = false;
                                        lessons.IsLocked = false;
                                        lessons.IsPending = true;
                                        lessons.IsProgressBar = true;
                                        lessons.LessonTextColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                                        lessons.LessonCountBackgroundColor = (Color)Application.Current.Resources["BlackText"];
                                        //REMARK: Handle the xp collected for each class depends on the correct answer of the class
                                        int classesEarnXP = chapters.LessonDataList.Where(p => p.Id == lessons.Id).Select(x => Convert.ToInt32(x.EarnXP)).FirstOrDefault();
                                        Utilities.ClassesXPCollected = classesEarnXP;
                                    }
                                    else if (lessons.Status == StringConstant.LessonComplete)
                                    {
                                        lessons.IsCompleted = true;
                                        lessons.IsPending = false;
                                        lessons.IsProgressBar = false;
                                        lessons.IsLocked = false;
                                        lessons.LessonTextColor = (Color)Application.Current.Resources["LessonXPFirstColor"];
                                        lessons.LessonCountBackgroundColor = (Color)Application.Current.Resources["BlackText"];
                                    }
                                    else if (lessons.Status == StringConstant.LessonsLocked)
                                    {
                                        lessons.IsCompleted = false;
                                        lessons.IsPending = false;
                                        lessons.IsProgressBar = false;
                                        lessons.IsLocked = true;
                                        lessons.LessonTextColor = (Color)Application.Current.Resources["LessonSegmentColor"];
                                        lessons.LessonCountBackgroundColor = (Color)Application.Current.Resources["LessonLockedGrayColor"];
                                    }
                                }
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, DFResources.SomethingWrongText, DFResources.OKText);
                        }

                    }
                    else if (lessonsResponseModel.StatusCode == 202 && !lessonsResponseModel.Status)
                    {
                        await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, lessonsResponseModel.Message, DFResources.OKText);
                    }
                    else
                    {
                        if (lessonsResponseModel.StatusCode == 501 || lessonsResponseModel.StatusCode == 401 || lessonsResponseModel.StatusCode == 400 || lessonsResponseModel.StatusCode == 404)
                        {
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ResponseErrorPage());
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, lessonsResponseModel.Message, DFResources.OKText);
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(DFResources.OopsText, DFResources.SomethingWrongText, DFResources.OKText);
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
        /// This method is used to unlock the chapter if completed using the unlock quiz API
        /// </summary>
        /// <param name="selectedQuiz"></param>
        /// <returns></returns>
        private async Task<ResponseModel> UnlockChapterQuiz(ChaptersModel selectedQuiz)
        {
            ResponseModel unlockChapterQuizResponseModel = new ResponseModel();
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                int userId = Preferences.Get(StringConstant.UserId, 0) == 0 ? 0 : Preferences.Get(StringConstant.UserId, 0);
                UnlockQuizRequestModel unlockChapterQuizRequestModel = new UnlockQuizRequestModel()
                {
                    UserId = userId,
                    ChapterId = selectedQuiz.Id,
                };
                unlockChapterQuizResponseModel = await aPIService.UnlockChapterQuizService(unlockChapterQuizRequestModel);
                if (unlockChapterQuizResponseModel != null)
                {
                    if (unlockChapterQuizResponseModel.StatusCode == 200 && unlockChapterQuizResponseModel.Status)
                    {
                    }
                    else if (unlockChapterQuizResponseModel.StatusCode == 202 && !unlockChapterQuizResponseModel.Status)
                    {
                        await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, unlockChapterQuizResponseModel.Message, DFResources.OKText);
                    }
                    else
                    {
                        if (unlockChapterQuizResponseModel.StatusCode == 501 || unlockChapterQuizResponseModel.StatusCode == 401 || unlockChapterQuizResponseModel.StatusCode == 400 || unlockChapterQuizResponseModel.StatusCode == 404)
                        {
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ResponseErrorPage());
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, unlockChapterQuizResponseModel.Message, DFResources.OKText);
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(DFResources.OopsText, DFResources.SomethingWrongText, DFResources.OKText);
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
            return unlockChapterQuizResponseModel;
        }
        
        
        
        #endregion
    }
}
