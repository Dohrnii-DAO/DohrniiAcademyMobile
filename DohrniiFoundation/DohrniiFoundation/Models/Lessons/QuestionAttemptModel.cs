using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    public class QuestionAttemptModel
    {
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public bool IsCorrect { get; set; }
        public int ClassId { get; set; }
        public int LessonId { get; set; }
        public int Xpcollected { get; set; }
    }
}
