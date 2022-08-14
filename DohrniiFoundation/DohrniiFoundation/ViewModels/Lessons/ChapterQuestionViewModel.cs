using Acr.UserDialogs;
using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Messages;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Views;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class ChapterQuestionViewModel : BaseViewModel
    {
        private readonly ILessonService _lessonService;
        private readonly IPlatformHelper _platformHelper;
        private static IAppState _appState;
        private readonly IMessenger _messenger;
        private readonly ICacheService _cacheService;
        public ChapterModel SelectedChapter { get; set; }
        public List<ChapterQuestionModel> Questions { get; set; }
        public ChapterQuestionModel SelectedQuestion { get; set; }
        public int PositionIndex { get; set; }
        public string BtnBG { get; set; } = StringConstant.TimerBG;
        public string TimerText { get; set; } = "0:00";
        public DateTime QuizDuration { get; set; }
        public bool ShowResult { get; set; }
        public string PercentageScore { get; set; } = "0%";
        public decimal ProgressValue { get; set; } = 0;
        public string QuestionNumber
        {
            get
            {
                return $"Question {PositionIndex + 1}";
            }
        }

        public override Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.DefaultStatusBarColor), true);
                        await Application.Current.MainPage.Navigation.PopModalAsync();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }

        public ChapterQuestionViewModel()
        {
            _lessonService = DependencyService.Get<ILessonService>();
            _platformHelper = DependencyService.Get<IPlatformHelper>();
            _appState = DependencyService.Get<IAppState>();
            _messenger = DependencyService.Get<IMessenger>();
            _cacheService = DependencyService.Get<ICacheService>();
            SelectedChapter = AppUtil.SelectedChapter;
            InitData();
        }

        private async void InitData()
        {
            try
            {
                await GetChapterQuestions(SelectedChapter.Id);
                PositionIndex = 0;
                if (Questions != null && Questions.Count > 0)
                {
                    this.SelectedQuestion = Questions[0];
                    var index = 0;
                    foreach (var item in this.SelectedQuestion.Options)
                    {
                        item.IsCurrentQtnTypeCloseEnded = this.SelectedQuestion.IsCloseEnded;
                        item.AlphabetOption = GetAlphabetText(index + 1, false);
                        index++;
                    }
                }

                var rst = await UserDialogs.Instance.ConfirmAsync(DFResources.StartQuizInfoText,DFResources.AlertText,DFResources.StartText,DFResources.CancelText);
                if (rst)
                {
                    QuizDuration = DateTime.UtcNow.AddMinutes(SelectedChapter.TimeLimit);
                    Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                    {
                        var timespan = QuizDuration - DateTime.UtcNow;
                        this.TimerText = (new DateTime() + timespan).ToString("mm:ss");
                        if (this.TimerText == "00:00")
                        {
                            if (!ShowResult)
                            {
                                CompleteQuiz();
                            }
                            return false;
                        }
                        return true;
                    });
                }
                else
                {
                    _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.DefaultStatusBarColor), true);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                
            }
            catch (Exception ex)
            {

                Crashes.TrackError(ex);
            }
        }

        public async Task GetChapterQuestions(int chapterId)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                var questions = await _lessonService.GetChapterQuestions(chapterId);
                if (questions != null)
                {
                    this.Questions = questions;
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }

        public async void OptionSelected(ClassQuestionOptionModel optionModel)
        {
            try
            {
                Questions[PositionIndex].AnsweredCorrect = optionModel.IsAnswer;
                Questions[PositionIndex].SelectedOption = optionModel;
                Questions[PositionIndex].CorrectOption = this.SelectedQuestion.Options.FirstOrDefault(c => c.IsAnswer == true);
                Questions[PositionIndex].IsAttempted = true;

                var attempt = new QuizAttempt
                {
                    QuestionId = SelectedQuestion.Id,
                    SelectedAnswerId = optionModel.Id,
                    ChapterId = SelectedQuestion.ChapterId,
                    IsCorrect = optionModel.IsAnswer
                };

                if (PositionIndex < Questions.Count)
                {
                    if (PositionIndex < (Questions.Count - 1))
                    {
                        this.SelectedQuestion = Questions[PositionIndex + 1];
                        var index = 0;
                        foreach (var item in this.SelectedQuestion.Options)
                        {
                            item.IsCurrentQtnTypeCloseEnded = this.SelectedQuestion.IsCloseEnded;
                            item.AlphabetOption = GetAlphabetText(index + 1, false);
                            index++;
                        }
                    }
                    _messenger.Send(new UpdateChapterDetailScreen());
                    var dd = await _lessonService.AttemptQuizQuestion(attempt);
                    if(dd != null)
                    {

                    }
                }
                PositionIndex++;
                if (PositionIndex == (Questions.Count))
                {
                    CompleteQuiz();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        public async void CompleteQuiz()
        {
            _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.primaryColor), true);
            if (Questions != null)
            {
                var correctCount = Questions.Where(c => c.AnsweredCorrect == true).Count();
                var QtnCount = Questions.Count();
                if (QtnCount > 0)
                {
                    var val = Convert.ToDecimal(correctCount) / Convert.ToDecimal(QtnCount);
                    PercentageScore = Math.Round(val * 100, MidpointRounding.ToEven).ToString() + "%";
                }
                else
                {
                    PercentageScore = "0%";
                }
            }
            else
            {
                PercentageScore = "0%";
            }
            if (Questions != null)
            {
                var correctCount = Questions.Where(c => c.AnsweredCorrect == true).Count();
                var QtnCount = Questions.Count();
                if (QtnCount > 0)
                {
                    ProgressValue = Convert.ToDecimal(correctCount) / Convert.ToDecimal(QtnCount);
                }
                else
                {
                    ProgressValue = 0;
                }
            }
            else
            {
                ProgressValue = 0;
            }

            this.ShowResult = true;

            var award = new AwardDhn();
            award.ChapterId = SelectedChapter.Id;
            var score = ProgressValue * 100;
            if (score > 90)
            {
                award.AwardedDhn = SelectedChapter.RewardHundred;
            }
            else if (score > 80)
            {
                award.AwardedDhn = SelectedChapter.RewardNinety;
            }
            else if (score > 70)
            {
                award.AwardedDhn = SelectedChapter.RewardEighty;
            }
            else
            {
                award.AwardedDhn = 0;
            }

            if (award.AwardedDhn > 0)
            {
                var resp = await _lessonService.AwardDHN(award);
                if (resp != null)
                {
                    AppUtil.CurrentUser = resp;
                    var login = await _cacheService.GetCurrentUser();
                    login.User = resp;
                    await _cacheService.SaveCurrentUser(login);
                    _messenger.Send(new UpdateChapterDetailScreen());
                    _messenger.Send(new UpdateLessonScreen());
                }
            }
        }
        private string GetAlphabetText(int number, bool isCaps)
        {
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }
    }
}
