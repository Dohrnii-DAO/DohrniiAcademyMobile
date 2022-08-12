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
        protected override void OnAppearing()
        {
            try
            {
                lessonsVM.SubscribeEvent();
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