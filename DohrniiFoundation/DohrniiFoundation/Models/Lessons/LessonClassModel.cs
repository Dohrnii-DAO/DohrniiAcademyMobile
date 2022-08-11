using DohrniiFoundation.Helpers;
using DohrniiFoundation.Resources;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class LessonClassModel
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int QuestionLimit { get; set; }
        public int XpPerQuestion { get; set; }
        public string HtmlContent { get; set; }
        public int Sequence { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsStarted { get; set; }
        public int TotalXP { get; set; }

        public string StackWidth
        {
            get
            {
                return Application.Current.MainPage.Width.ToString();
            }

        }
        public string BorderColor
        {
            get
            {
                if (IsCompleted)
                    return StringConstant.primaryColor;
                else if (IsStarted)
                    return StringConstant.primaryColor;
                else
                    return StringConstant.Grey2;
            }
        }

        public string StatusText
        {
            get
            {
                if (IsCompleted)
                    return DFResources.PassedText;
                else if (IsStarted)
                    return DFResources.UnlockedText;
                else
                    return DFResources.LockedText;
            }
        }

        public string TextColor
        {
            get
            {
                if (IsCompleted)
                    return StringConstant.primaryColor;
                else if (IsStarted)
                    return StringConstant.primaryColor;
                else
                    return StringConstant.BorderColor2;
            }
        }
        public string CircleBGColor
        {
            get
            {
                if (IsCompleted)
                    return StringConstant.WhiteColor;
                else if (IsStarted)
                    return StringConstant.primaryColor;
                else
                    return StringConstant.WhiteColor;
            }
        }
        public string StatusImg
        {
            get
            {
                if (IsCompleted)
                    return "mcheck.png";
                else if (IsStarted)
                    return "";
                else
                    return "mlock.png";
            }
        }
        public bool ShowStatusImg
        {
            get
            {
                if (IsCompleted)
                    return true;
                else if (IsStarted)
                    return false;
                else
                    return true;
            }
        }
        public bool ShowScareResources
        {
            get
            {
                if (IsCompleted)
                    return true;
                else if (IsStarted)
                    return true;
                else
                    return false;
            }
        }
    }
}
