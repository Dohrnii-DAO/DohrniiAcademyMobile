using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Messages;
using DohrniiFoundation.Models;
using DohrniiFoundation.Models.APIRequestModel.Lessons;
using DohrniiFoundation.Models.APIResponseModels.Lessons;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.Lessons;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class ClassesQuestionViewModel : BaseViewModel
    {
        //public Color CheckFrameBgColor { get; set; }
        private readonly ILessonService _lessonService;
        private readonly IPlatformHelper _platformHelper;
        private static IAppState _appState;
        private readonly IMessenger _messenger;
        public ICommand CheckAndContinueCommand { get; set; }
        public ICommand ContinueToClassCommand { get; set; }
        public LessonClassModel SelectedClass { get; set; }
        public ClassQuestionModel SelectedQuestion { get; set; }
        public ClassQuestionOptionModel UserSelectedOption { get; set; }
        public List<ClassQuestionModel> Questions { get; set; }
        public string ResultText { get; set; }
        public int PositionIndex { get; set; }
        public bool IsChecked { get; set; }
        public string BGColor { get; set; } = StringConstant.DefaultQtnBG;
        public string BtnBG { get; set; } = StringConstant.BorderColor2;
        public string BtnText { get; set; } = DFResources.CheckText;
        public int FiveInRowCount { get; set; } = 0;
        public int FourInRowCount { get; set; } = 0;
        public bool ShowClassComplete { get; set; }
        public int TotalXpEarned { get; set; }
        public double LessonProgress { get; set; }
        public bool IsCompleted { get; set; }
        public string QuestionNumber
        {
            get
            {
                return $"Question {PositionIndex + 1}";
            }
        }
        public string QuestionNumberTextColor
        {
            get
            {
                if(BtnBG == StringConstant.BorderColor2)
                {
                    return StringConstant.primaryColor;
                }
                else
                {
                    return BtnBG;
                }
            }
        }
        public bool ShowFiveInRowCount
        {
            get
            {
                if(FiveInRowCount >= 5)
                {
                    return true;
                }
                return false;
            }
        }
        public bool ShowFourInRowCount
        {
            get
            {
                if (FourInRowCount >= 4)
                {
                    return true;
                }
                return false;
            }
        }

        public ClassesQuestionViewModel()
        {
            _lessonService = DependencyService.Get<ILessonService>(); //new LessonService();
            _platformHelper = DependencyService.Get<IPlatformHelper>();
            _appState = DependencyService.Get<IAppState>();
            _messenger = DependencyService.Get<IMessenger>();
            SelectedClass = AppUtil.SelectedClass;
            CheckAndContinueCommand = new Command(CheckAndContinueClick);
            ContinueToClassCommand = new Command(ContinueToClassClick);
            InitData();
        }

        private async void InitData()
        {
            await GetClassQuestions(SelectedClass.Id);
            PositionIndex = 0;
            if(Questions != null && Questions.Count > 0)
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
        }

        public async Task GetClassQuestions(int classId)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });
                var questions = await _lessonService.GetQuestions(classId);
                if (questions != null)
                {
                    this.Questions = questions;
                    _appState.Questions = questions;
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

        public void OptionSelected(ClassQuestionOptionModel optionModel)
        {
            try
            {
                if (!IsChecked)
                {
                    foreach (var item in this.SelectedQuestion.Options)
                    {
                        item.IsSelected = false;
                        item.CorrectAnswer = this.SelectedQuestion.Options.FirstOrDefault(c => c.IsAnswer == true)?.AnswerOption;
                        item.CorrectAnswerAlphabet = this.SelectedQuestion.Options.FirstOrDefault(c => c.IsAnswer == true)?.AlphabetOption;
                    }
                    optionModel.IsCurrentQtnTypeCloseEnded = this.SelectedQuestion.IsCloseEnded;
                    optionModel.IsSelected = true;
                    this.UserSelectedOption = optionModel;
                    this.BtnBG = StringConstant.primaryColor;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void ContinueToClassClick()
        {
            _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.DefaultStatusBarColor), false);
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public async void CheckAndContinueClick()
        {
            try
            {
                if(this.BtnBG != StringConstant.BorderColor2)
                {
                    if (this.BtnText == DFResources.CheckText)
                    {
                        this.BtnText = DFResources.ContinueText;
                        this.IsChecked = true;
                        this.UserSelectedOption.IsChecked = true;
                        var ca = this.SelectedQuestion.Options.FirstOrDefault(c => c.IsAnswer == true);
                        if(ca != null)
                        {
                            ca.IsChecked = true;
                        }

                        var attempt = new QuestionAttemptModel
                        {
                            ClassId = SelectedClass.Id,
                            LessonId = SelectedClass.LessonId,
                            QuestionId = SelectedQuestion.Id,
                            SelectedAnswerId = UserSelectedOption.Id,
                        };

                        if (this.UserSelectedOption.IsAnswer)
                        {
                            var qtn = _appState.Questions.FirstOrDefault(c => c.Id == SelectedQuestion.Id);
                            if(qtn != null)
                            {
                                if (qtn.Attempts.Count <= 0)
                                {
                                    attempt.Xpcollected = SelectedClass.XpPerQuestion;
                                }
                            }
                            
                            attempt.IsCorrect = true;

                            BGColor = StringConstant.CorrectAnsBG;
                            this.ResultText = GetResultText();
                            this.FiveInRowCount++;
                            this.FourInRowCount++;
                            if(ShowFiveInRowCount || ShowFourInRowCount)
                            {
                                this.BtnBG = StringConstant.primaryColor;
                            }
                            else
                            {
                                this.BtnBG = StringConstant.CorrectAnsColor;
                            }
                            _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.CorrectAnsBG), true);
                        }
                        else
                        {
                            attempt.IsCorrect = false;

                            BGColor = StringConstant.WrongAnsBG;
                            this.ResultText = DFResources.CorrectAnswerText;
                            this.BtnBG = StringConstant.WrongAnsColor;
                            this.FiveInRowCount = 0;
                            this.FourInRowCount = 0;
                            _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.WrongAnsBG), true);
                        }
                        var correctAttempt = this.SelectedQuestion.Attempts.FirstOrDefault(c=>c.IsCorrect);
                        if(correctAttempt == null)
                        {
                            var recordAttempt = await _lessonService.AttemptQuestion(attempt);
                            if (recordAttempt != null)
                            {
                                var qtn = _appState.Questions.FirstOrDefault(c => c.Id == attempt.QuestionId);
                                if (qtn != null)
                                {
                                    this.SelectedQuestion.Attempts.Add(new ClassQuestionAttemptModel { DateAttempt = DateTime.UtcNow, IsCorrect = attempt.IsCorrect, QuestionId = attempt.QuestionId, SelectedAnswerId = attempt.SelectedAnswerId, Xpcollected = attempt.Xpcollected, UserId = AppUtil.CurrentUser.Id });
                                }
                            }
                        }
                        this.SelectedQuestion.IsAttempted = true;
                    }
                    else
                    {
                        if (PositionIndex < (Questions.Count - 1))
                        {
                            PositionIndex++;
                            this.IsChecked = false;
                            this.UserSelectedOption = null;
                            BGColor = StringConstant.DefaultQtnBG;
                            this.ResultText = String.Empty;
                            this.BtnBG = StringConstant.BorderColor2;
                            this.BtnText = DFResources.CheckText;
                            this.SelectedQuestion = Questions[PositionIndex];
                            var index = 0;
                            foreach (var item in this.SelectedQuestion.Options)
                            {
                                item.IsCurrentQtnTypeCloseEnded = this.SelectedQuestion.IsCloseEnded;
                                item.AlphabetOption = GetAlphabetText(index + 1, false);
                                index++;
                            }
                            if (ShowFourInRowCount)
                            {
                                this.FourInRowCount = 0;
                            }
                            if (ShowFiveInRowCount)
                            {
                                this.FiveInRowCount = 0;
                            }
                            _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.DefaultStatusBarColor), true);
                        }
                        else
                        {
                            this.IsCompleted = false;
                            this.ShowClassComplete = true;
                            _platformHelper.SetStatusBarColor(ColorConverters.FromHex(StringConstant.LightBlueStatusBarColor), true);
                            var lc = AppUtil.SelectedLesson.Classes.FirstOrDefault(c => c.Id == this.SelectedClass.Id);
                            if (lc != null)
                            {
                                lc.IsCompleted = true;
                                if (AppUtil.SelectedLesson.Classes.Count > lc.Sequence)
                                {
                                    AppUtil.SelectedLesson.Classes[lc.Sequence].IsStarted = true;
                                }
                            }
                            var complete = new CompleteRequestModel { Id = this.SelectedClass.Id };
                            var completed = await _lessonService.CompleteClass(complete);
                            if (completed != null)
                            {
                                this.TotalXpEarned = completed.TotalXpEarned;
                                this.LessonProgress = completed.PercentageComplete;
                                var lesson = _appState.ChapterDetail.Lessons.FirstOrDefault(c => c.Id == SelectedClass.LessonId);
                                if (lesson != null)
                                {
                                    var lessonClass = lesson.Classes.FirstOrDefault(c => c.Id == this.SelectedClass.Id);
                                    if (lessonClass != null)
                                    {
                                        lessonClass.IsCompleted = true;
                                        if (lesson.Classes.Count > lc.Sequence)
                                        {
                                            lesson.Classes[lc.Sequence].IsStarted = true;
                                            var sc = _lessonService.StartClass(new StartRequestModel { Id = lesson.Classes[lc.Sequence].Id });
                                        }
                                        else
                                        {
                                            lesson.IsCompleted = true;
                                            var completeLesson = new CompleteRequestModel { Id = this.SelectedClass.LessonId };
                                            var completedLesson = await _lessonService.CompleteLesson(completeLesson);
                                            if (_appState.ChapterDetail.Lessons.Count > lesson.Sequence)
                                            {
                                                _appState.ChapterDetail.Lessons[lesson.Sequence].IsStarted = true;
                                                var sl = _lessonService.StartLesson(new StartRequestModel { Id = _appState.ChapterDetail.Lessons[lesson.Sequence].Id });
                                            }
                                        }
                                    }
                                }
                            }
                            this.IsCompleted = true;
                        }

                        _messenger.Send(new UpdateChapterDetailScreen());
                        _messenger.Send(new UpdateLessonScreen());
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private string GetResultText()
        {
            var random = new Random();
            var list = new List<string> { DFResources.ExcellentText, DFResources.GoodJobText, DFResources.NiceText, DFResources.NicelyDoneText, DFResources.NiceJobText, DFResources.GreatJobText, DFResources.AwesomeText, DFResources.AmazingText, DFResources.CorrectText };
            int index = random.Next(list.Count);
            return list[index];
        }
        private string GetAlphabetText(int number, bool isCaps)
        {
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }
    }
}


