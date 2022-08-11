using DohrniiFoundation.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.IServices
{
    public interface IAppState
    {
        ChapterModel ChapterDetail { get; set; }
        List<ClassQuestionModel> Questions { get; set; }
    }
}
