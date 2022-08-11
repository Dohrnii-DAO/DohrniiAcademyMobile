using DohrniiFoundation.Models.Lessons;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DohrniiFoundation.Views.Lessons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonsDetailPage : ContentPage
    {
        public LessonsDetailPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            VM.ViewAppearing();
        }
        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            try
            {
                //ClassModel currentItem = e.CurrentItem as ClassModel;
                //if (currentItem != null)
                //{
                //    if (VM != null)
                //    {
                //        VM.SelectedClassNumber = currentItem.ClassNumber;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if(VM != null)
            {
                VM.HideLoading();
            }
        }
    }
}