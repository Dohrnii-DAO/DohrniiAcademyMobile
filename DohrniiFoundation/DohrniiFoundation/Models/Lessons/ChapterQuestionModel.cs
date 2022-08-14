using DohrniiFoundation.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class ChapterQuestionModel
    {
        public int Id { get; set; }
        public int LessonClassId { get; set; }
        public int LessonId { get; set; }
        public int ChapterId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string Tags { get; set; }
        public bool IsAttempted { get; set; }
        public List<ClassQuestionOptionModel> Options { get; set; }
        public List<ClassQuestionAttemptModel> Attempts { get; set; }

        public bool AnsweredCorrect { get; set; }
        public ClassQuestionOptionModel SelectedOption { get; set; }
        public ClassQuestionOptionModel CorrectOption { get; set; }


        public bool IsCloseEnded
        {
            get
            {
                return QuestionType.ToLower() == "close ended";
            }

        }
        public bool IsMultipleChoice
        {
            get
            {
                return QuestionType.ToLower() == "multiple choice";
            }

        }
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
                if (IsAttempted)
                    return StringConstant.primaryColor;
                else
                    return StringConstant.Grey2;
            }
        }
        public string CircleBGColor
        {
            get
            {
                if (IsAttempted)
                {
                    var attempt = Attempts.OrderByDescending(c => c.DateAttempt).FirstOrDefault();
                    if (attempt != null)
                    {
                        if (attempt.IsCorrect)
                        {
                            return StringConstant.CorrectAnsColor;
                        }
                    }
                    return StringConstant.WrongAnsColor;
                }
                else
                {
                    return StringConstant.WhiteColor;
                }
            }
        }

        public string QuizCircleBGColor
        {
            get
            {
                if (IsAttempted)
                {
                    if (AnsweredCorrect)
                    {
                        return StringConstant.CorrectAnsColor;
                    }
                    return StringConstant.WrongAnsColor;
                }
                else
                {
                    return StringConstant.WhiteColor;
                }
            }
        }
    }
}
