using DohrniiFoundation.Helpers;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DohrniiFoundation.Views.Lessons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonChaptersPage : ContentPage
    {
        #region Constructor
        public LessonChaptersPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            lessonChaptersVM.GetChapterDetails(AppUtil.CurrentLessonInprogress.ChapterId);
        }

        private void LessonsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ClassesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        #endregion
    }
}