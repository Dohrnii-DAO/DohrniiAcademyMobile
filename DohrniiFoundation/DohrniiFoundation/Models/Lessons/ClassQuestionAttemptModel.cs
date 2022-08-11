using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    public class ClassQuestionAttemptModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public int UserId { get; set; }
        public bool IsCorrect { get; set; }
        public int Xpcollected { get; set; }
        public int SelectedOption { get; set; }
        public DateTime DateAttempt { get; set; }
    }
}
