using DohrniiFoundation.Models;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DohrniiFoundation.IServices
{
    public interface ILessonService
    {
        Task<List<CategoryModel>> GetCategories();
        Task<UserStatus> GetUserStatus();
        Task<CategoryProgress> GetCategoryProgress(int id);
        Task<ChapterModel> GetChapter(int id);
        Task<StartResponseModel> StartLesson(StartRequestModel model);
        Task<CompleteResponseModel> CompleteLesson(CompleteRequestModel model);
        Task<List<ClassQuestionModel>> GetQuestions(int classId);
        Task<StartResponseModel> StartClass(StartRequestModel model);
        Task<CompleteResponseModel> CompleteClass(CompleteRequestModel model);
        Task<QuestionAttemptResp> AttemptQuestion(QuestionAttemptModel model);
        Task<User> ConvertXptoJelly(XPtoJellyModel model);
        Task<User> UnlockChapterQuiz(UnlockQuizModel model);
        Task<List<ChapterQuestionModel>> GetChapterQuestions(int chapterId);
        Task<QuestionAttemptResp> AttemptQuizQuestion(QuizAttempt model);
        Task<User> AwardDHN(AwardDhn model);

    }
}
