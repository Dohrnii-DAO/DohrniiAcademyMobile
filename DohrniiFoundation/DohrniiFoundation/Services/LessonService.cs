using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models;
using DohrniiFoundation.Models.APIResponseModels;
using DohrniiFoundation.Models.Lessons;
using DohrniiFoundation.Models.UserModels;
using DohrniiFoundation.Resources;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DohrniiFoundation.Services
{
    public class LessonService : ILessonService
    {
        #region Private Members
        private static ServiceHelpers serviceHelpers;
        #endregion

        #region Constructor
        public LessonService()
        {
            serviceHelpers = ServiceHelpers.Instance;
        }

        #endregion


        public async Task<List<CategoryModel>> GetCategories()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetCategoriesEndPoint, true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    categories = JsonConvert.DeserializeObject<List<CategoryModel>>(response.Data);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return categories;
        }

        public async Task<UserStatus> GetUserStatus()
        {
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetUserStatusEndPoint, true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    var userStatus = JsonConvert.DeserializeObject<UserStatus>(response.Data);
                    return userStatus;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return null;
        }

        public async Task<CategoryProgress> GetCategoryProgress(int id)
        {
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetCategoryProgressEndPoint.Replace("{id}", id.ToString()), true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    var userStatus = JsonConvert.DeserializeObject<CategoryProgress>(response.Data);
                    return userStatus;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return null;
        }

        public async Task<ChapterModel> GetChapter(int id)
        {
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetChapterDetailEndPoint.Replace("{id}", id.ToString()), true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    var chapter = JsonConvert.DeserializeObject<ChapterModel>(response.Data);
                    return chapter;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return null;
        }

        public async Task<StartResponseModel> StartLesson(StartRequestModel model)
        {
            StartResponseModel resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.StartLessonEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<StartResponseModel>(response.Data);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<CompleteResponseModel> CompleteLesson(CompleteRequestModel model)
        {
            CompleteResponseModel resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PutRequestAsync(serializedRequest, StringConstant.CompleteLessonEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<CompleteResponseModel>(response.Data);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<List<ClassQuestionModel>> GetQuestions(int classId)
        {
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetQuestionsEndPoint.Replace("{id}", classId.ToString()), true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    var questions = JsonConvert.DeserializeObject<List<ClassQuestionModel>>(response.Data);
                    return questions;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return null;
        }

        public async Task<StartResponseModel> StartClass(StartRequestModel model)
        {
            StartResponseModel resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.StartClassEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<StartResponseModel>(response.Data);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<CompleteResponseModel> CompleteClass(CompleteRequestModel model)
        {
            CompleteResponseModel resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PutRequestAsync(serializedRequest, StringConstant.CompleteClassEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<CompleteResponseModel>(response.Data);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<QuestionAttemptResp> AttemptQuestion(QuestionAttemptModel model)
        {
            QuestionAttemptResp resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.QuetionAttemptEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<QuestionAttemptResp>(response.Data); ;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<User> ConvertXptoJelly(XPtoJellyModel model)
        {
            User resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.XPtoJellyEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<User>(response.Data); ;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<User> UnlockChapterQuiz(UnlockQuizModel model)
        {
            User resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.UnlockQuizEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<User>(response.Data); ;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<List<ChapterQuestionModel>> GetChapterQuestions(int chapterId)
        {
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetChapterQuestionsEndPoint.Replace("{id}", chapterId.ToString()), true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    var questions = JsonConvert.DeserializeObject<List<ChapterQuestionModel>>(response.Data);
                    return questions;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return null;
        }
        public async Task<QuestionAttemptResp> AttemptQuizQuestion(QuizAttempt model)
        {
            QuestionAttemptResp resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.QuizAttemptEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<QuestionAttemptResp>(response.Data); ;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }

        public async Task<User> AwardDHN(AwardDhn model)
        {
            User resp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(model);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.AwardDHNEndPoint, true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    resp = JsonConvert.DeserializeObject<User>(response.Data); ;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return resp;
        }
    }
}
