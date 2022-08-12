using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Models.Socials;
using DohrniiFoundation.Models.UserModels;
using Microsoft.AppCenter.Crashes;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace DohrniiFoundation.Helpers
{
    [AddINotifyPropertyChangedInterface]
    public class AppUtil
    {
        public static string AccessToken
        {
            get
            {
                return Preferences.Get("accessToken", string.Empty);
            }
        }
        public static AppUser CurrentAppUser { get; set; }
        public static User CurrentUser { get; set; }
        public static CategoryModel ChaptersCategorySelected { get; set; }
        public static LessonInprogress CurrentLessonInprogress { get; set; }
        public static LessonModel SelectedLesson { get; set; }
        public static LessonClassModel SelectedClass { get; set; }
        public static string SelectedLessonTypePoints { get; set; }






        public static bool IsValidEmailAddress(string value)
        {
            try
            {
                bool isEmail = Regex.IsMatch(value.Trim(), StringConstant.EmailRegex);
                return isEmail;
            }
            catch (FormatException ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
        }


        public static bool IsValidPhoneNumber(string value)
        {
            try
            {
                bool isPhone = Regex.IsMatch(value.Trim(), StringConstant.PhoneRegex);
                return isPhone;
            }
            catch (FormatException ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
        }

        public static bool IsValidPassword(string value)
        {
            try
            {
                bool isPassword = Regex.IsMatch(value.Trim(), StringConstant.passwordRegex);
                return isPassword;
            }
            catch (FormatException ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
        }
    }
}
