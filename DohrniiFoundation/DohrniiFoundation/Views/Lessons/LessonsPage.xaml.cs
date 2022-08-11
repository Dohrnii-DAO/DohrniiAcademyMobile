using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DohrniiFoundation.Views.Lessons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonsPage : ContentPage
    {
        public LessonsPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await GetStatus();
        }
        private async Task GetStatus()
        {
            try
            {
                await lessonsVM.initData();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}