using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Services
{
    public class AppState : IAppState
    {
        public ChapterModel ChapterDetail { get; set; } = new ChapterModel();
        public List<ClassQuestionModel> Questions { get; set; } = new List<ClassQuestionModel>();
    }
}
