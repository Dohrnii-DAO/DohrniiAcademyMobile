using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Socials
{
    public class UserProfileViewModel : BaseViewModel
    {
        private static ISocialService socialService;

        public override Command BackCommand => OnBackCommand;

        public Command OnBackCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        Application.Current.MainPage.Navigation.PopAsync();
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
