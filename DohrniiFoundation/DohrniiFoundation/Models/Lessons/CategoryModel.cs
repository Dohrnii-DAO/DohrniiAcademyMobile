using DohrniiFoundation.Helpers;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public string BGColor
        {
            get
            {
                if (IsSelected)
                {
                    return StringConstant.BorderColor2;
                }
                else
                {
                    return StringConstant.Transparent;
                }
            }
        }
    }
}
