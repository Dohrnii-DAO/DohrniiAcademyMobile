using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class CategoryProgress
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int TotalLesson { get; set; }
        public int CompletedLesson { get; set; }
        public int TotalClass { get; set; }
        public int CompletedClass { get; set; }
    }
}
