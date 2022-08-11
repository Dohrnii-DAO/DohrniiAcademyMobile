using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.Lessons
{
    public class QuestionAttemptResp
    {
        public int QuestionId { get; set; }
        public int AnwserId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
