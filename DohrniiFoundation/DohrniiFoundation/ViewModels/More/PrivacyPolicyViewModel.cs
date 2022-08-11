using DohrniiFoundation.Helpers;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.More
{
    [AddINotifyPropertyChangedInterface]
    public class PrivacyPolicyViewModel: BaseViewModel
    {
        public PrivacyPolicyViewModel()
        {

        }
        public override Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
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
