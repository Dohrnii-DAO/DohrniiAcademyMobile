using DohrniiFoundation.Models.More;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DohrniiFoundation.Views.More
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileMorePage : ContentPage
    {
        public ProfileMorePage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected async override void OnAppearing()
        {
            await VM.GetProfileDetails();
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            try
            {
                ProfileMoreModel selectedItem = (ProfileMoreModel)((Frame)sender).BindingContext;
                if(selectedItem != null)
                {
                    VM.GetSelectedItem(selectedItem);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}