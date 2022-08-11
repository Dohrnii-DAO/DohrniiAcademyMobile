using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Services;
using DohrniiFoundation.Views.Lessons;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class LessonDetailViewModel: BaseViewModel
    {
        private static ILessonService _lessonService;
        public ICommand PreviousCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public LessonModel SelectedLesson { get; set; }
        public LessonClassModel SelectedClass { get; set; }
        public int PositionIndex { get; set; }
        public bool ShowLoader { get; set; } = true;
        public string ClassPagination { 
            get {
                return $"{SelectedClass.Sequence}/{SelectedLesson.Classes.Count}";
            } 
        }
        public LessonDetailViewModel()
        {
            _lessonService = DependencyService.Get<ILessonService>(); //new LessonService();
            SelectedLesson = AppUtil.SelectedLesson;
            SelectedClass = AppUtil.SelectedClass;
            PositionIndex = SelectedClass.Sequence - 1;
            
            PreviousCommand = new Command(PreviousClick);
            NextCommand = new Command(NextClick);
        }


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

        public void ViewAppearing()
        {
            var currentClass = SelectedLesson.Classes.FirstOrDefault(c => c.IsStarted == true && c.IsCompleted == false);
            if (currentClass != null)
            {
                SelectedClass = currentClass;
                PositionIndex = currentClass.Sequence - 1;
            }
            else
            {
                PositionIndex = SelectedClass.Sequence - 1;
            }
        }

        private void PreviousClick()
        {
            try
            {
                if(PositionIndex > 0)
                {
                    PositionIndex--;
                    SelectedClass = SelectedLesson.Classes[PositionIndex];
                    AppUtil.SelectedClass = SelectedClass;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                IsLoading = false;
            }
        }
        private void NextClick()
        {
            try
            {
                if (PositionIndex < (SelectedLesson.Classes.Count - 1))
                {
                    PositionIndex++;
                    SelectedClass = SelectedLesson.Classes[PositionIndex];
                    AppUtil.SelectedClass = SelectedClass;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                IsLoading = false;
            }
        }
        public void HideLoading()
        {
            ShowLoader = false;
        }

        public Command QuestionsCommand
        {
            get
            {
                return new Command(async (param) =>
                {
                    try
                    {
                        var selectedLesson = param as LessonClassModel;
                        AppUtil.SelectedClass = selectedLesson;
                        await Application.Current.MainPage.Navigation.PushModalAsync(new ClassesQuestionPage());

                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }
    }
}
