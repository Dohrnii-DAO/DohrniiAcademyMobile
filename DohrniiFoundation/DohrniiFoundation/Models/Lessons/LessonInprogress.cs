using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class LessonInprogress
    {
        public int CategoryId { get; set; }
        public int ChapterId { get; set; }

        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public bool IsNotStarted { get; set; }
    }
}
