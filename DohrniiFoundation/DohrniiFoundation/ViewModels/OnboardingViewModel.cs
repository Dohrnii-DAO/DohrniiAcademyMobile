using DohrniiFoundation.Helpers;
using DohrniiFoundation.Models;
using DohrniiFoundation.Resources;
using DohrniiFoundation.Views;
using DohrniiFoundation.Views.User;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class OnboardingViewModel: BaseViewModel
    {

        public ICommand NextCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public List<Boarding> Boardings { get; set; }
        public int PositionIndex { get; set; }
        public string LoginText { get; set; } = DFResources.LoginText;
        public string ButtonText
        {
            get
            {
                if (PositionIndex == Boardings.Count - 1)
                    return DFResources.RegistrationText;
                return DFResources.NextText;
            }
        }
        public bool ShowLogin
        {
            get
            {
                if (PositionIndex == Boardings.Count - 1)
                    return true;
                return false;
            }
        }
        public string Name { get; set; }


        public OnboardingViewModel()
        {
            Boardings = new List<Boarding>
            {
                new Boarding
                {
                    Body = DFResources.AboutApp,
                    Header = DFResources.BoardingHeader1,
                    Illustration = "slide1.png",
                    Title = DFResources.BoardingTitle1,
                    Token = String.Empty,
                },
                new Boarding
                {
                    Body = DFResources.XPBody,
                    Header = DFResources.BoardingHeader2,
                    Illustration = "slide2.png",
                    Title = DFResources.WhatisXPText,
                    Token = $"{DFResources.XPText} - ",
                },
                new Boarding
                {
                    Body = DFResources.JellyBody,
                    Header = DFResources.BoardingHeader3,
                    Illustration = "slide3.png",
                    Title = DFResources.WhatisCryptoJellyText,
                    Token = $"{DFResources.JellyText} - ",
                },
                new Boarding
                {
                    Body = DFResources.DHNBody,
                    Header = DFResources.BoardingHeader4,
                    Illustration = "slide4.png",
                    Title = DFResources.WhatisDHNText,
                    Token = $"{DFResources.DHNText} - ",
                }
            };
            NextCommand = new Command(Next);
            LoginCommand = new Command(Login);

        }

        private void Next(object obj)
        {
            if (PositionIndex < Boardings.Count - 1)
            {
                PositionIndex++;
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new RegisterPage());
            }
        }

        private void Login()
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
