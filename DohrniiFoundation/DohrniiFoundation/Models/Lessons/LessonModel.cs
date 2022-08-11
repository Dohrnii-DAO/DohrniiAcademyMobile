using DohrniiFoundation.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class LessonModel
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public int TotalClass { get; set; }
        public int CompletedClass { get; set; }
        public bool IsStarted { get; set; }
        public bool IsCompleted { get; set; }
        public int TotalXPEarned { get; set; }
        public int TotalJellyEarned { get; set; }
        public int TotalXP { get; set; }
        public List<LessonClassModel> Classes { get; set; }

        //Local use

        public string SequenceBG { 
            get {
                if (IsCompleted)
                    return StringConstant.Black;
                else if (IsStarted)
                    return StringConstant.Black;
                else
                    return StringConstant.Grey2;
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
        public string StatusImg
        {
            get
            {
                if (IsCompleted)
                    return "mLessonDone.png";
                else if (IsStarted)
                    return "";
                else
                    return "mLessonLocked.png";
            }
        }

        public bool ShowPercentage
        {
            get
            {
                if (IsCompleted)
                    return false;
                else if (IsStarted)
                    return true;
                else
                    return false;
            }
        }
        public string PercentageComplete
        {
            get
            {
                decimal classCompleted = Convert.ToDecimal(CompletedClass);
                decimal classesTotal = Convert.ToDecimal(TotalClass);
                if(TotalClass != 0)
                {
                    decimal data = classCompleted / classesTotal;
                    return $"{Math.Round(data * 100, MidpointRounding.ToEven)}%";
                }
                else
                {
                    return "0%";
                }
            }
        }

        public decimal PercentagePerXPCollected
        {
            get
            {
                decimal totalXPEarned = Convert.ToDecimal(TotalXPEarned);
                decimal totalXP = Convert.ToDecimal(TotalXP);
                if (totalXP != 0)
                {
                    return totalXPEarned / totalXP;
                    //return Math.Round(data * 100, MidpointRounding.ToEven);
                }
                else
                {
                    return Convert.ToDecimal(0); ;
                }
            }
        }

        public string ClassCount
        {
            get
            {
                if(Classes.Count > 0)
                {
                    return $"1/{Classes.Count}";
                }
                return "0/0";
            }
        }
    }
}
