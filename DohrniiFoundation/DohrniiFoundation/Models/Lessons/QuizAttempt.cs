using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    public class QuizAttempt
    {
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public bool IsCorrect { get; set; }
        public int Xpcollected { get; set; }
        public int ChapterId { get; set; }
    }
}
