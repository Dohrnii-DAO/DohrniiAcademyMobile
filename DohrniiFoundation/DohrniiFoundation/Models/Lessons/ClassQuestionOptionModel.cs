using DohrniiFoundation.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class ClassQuestionOptionModel
    {
        public int Id { get; set; }
        public int ClassQuestionId { get; set; }
        public string AnswerOption { get; set; }
        public bool IsAnswer { get; set; }


        public string CorrectAnswer { get; set; }
        public string CorrectAnswerAlphabet { get; set; }
        public string AlphabetOption { get; set; }
        public bool IsCurrentQtnTypeCloseEnded { get; set; }
        public bool IsSelected { get; set; }
        public bool IsChecked { get; set; }
        public string BGColor
        {
            get
            {
                if (IsSelected)
                {
                    if (IsChecked)
                    {
                        if (IsCurrentQtnTypeCloseEnded)
                        {
                            if (IsAnswer)
                            {
                                return StringConstant.CorrectAnsColor;
                            }
                            else
                            {
                                return StringConstant.WrongAnsColor;
                            }
                        }
                        else
                        {
                            return StringConstant.WhiteColor;
                        }
                    }
                    else
                    {
                        if (IsCurrentQtnTypeCloseEnded)
                        {
                            return StringConstant.primaryColor;
                        }
                        else
                        {
                            return StringConstant.WhiteColor;
                        }
                    }
                }
                return StringConstant.WhiteColor;
            }
        }
        public string TextColor
        {
            get
            {
                if (IsSelected)
                {
                    if (IsChecked)
                    {
                        if (!IsCurrentQtnTypeCloseEnded)
                        {
                            if (IsAnswer)
                            {
                                return StringConstant.CorrectAnsColor;
                            }
                            else
                            {
                                return StringConstant.WrongAnsColor;
                            }
                        }
                        else
                        {
                            return StringConstant.WhiteColor;
                        }
                    }
                    else
                    {
                        if (!IsCurrentQtnTypeCloseEnded)
                        {
                            return StringConstant.primaryColor;
                        }
                        else
                        {
                            return StringConstant.WhiteColor;
                        }
                    }
                }
                if (IsCurrentQtnTypeCloseEnded)
                {
                    return StringConstant.primaryColor;
                }
                else
                {
                    return StringConstant.DarkText70;
                }
            }
        }

        public string AlphabetBorderColor
        {
            get
            {
                if(BGColor == StringConstant.DarkText70)
                {
                    return StringConstant.primaryColor;
                }
                else
                {
                    if(TextColor == StringConstant.DarkText70)
                    {
                        if (IsChecked)
                        {
                            if (IsAnswer)
                            {
                                return StringConstant.CorrectAnsColor;
                            }
                        }
                        return StringConstant.primaryColor;
                    }
                    else
                    {
                        return TextColor;
                    }
                }
            }
        }
        public string AlphabetBGColor
        {
            get
            {
                if (BGColor == StringConstant.DarkText70)
                {
                    return StringConstant.WhiteColor;
                }
                else
                {
                    if (TextColor == StringConstant.DarkText70)
                    {
                        return StringConstant.WhiteColor;
                    }
                    else
                    {
                        return TextColor;
                    }
                }
            }
        }
        public string AlphabetTextColor
        {
            get
            {
                if (BGColor == StringConstant.DarkText70)
                {
                    if (IsChecked)
                    {
                        if (IsAnswer)
                        {
                            return StringConstant.CorrectAnsColor;
                        }
                    }
                    return StringConstant.primaryColor;
                }
                else
                {
                    if (IsSelected)
                    {
                        return StringConstant.WhiteColor;
                    }
                    else
                    {
                        if (TextColor == StringConstant.DarkText70)
                        {
                            if (IsChecked)
                            {
                                if (IsAnswer)
                                {
                                    return StringConstant.CorrectAnsColor;
                                }
                            }
                            return StringConstant.primaryColor;
                        }
                        else
                        {
                            return TextColor;
                        }
                    }
                }
            }
        }

        public bool ShowCorrectAlphabet
        {
            get
            {
                if (IsChecked)
                {
                    if (!IsCurrentQtnTypeCloseEnded)
                    {
                        if (!IsAnswer)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public bool ShowCorrectTrue
        {
            get
            {
                if (IsChecked)
                {
                    if (IsCurrentQtnTypeCloseEnded)
                    {
                        if (!IsAnswer)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}
