using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.APIResponseModels;
using DohrniiFoundation.Models.Socials;
using DohrniiFoundation.Resources;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DohrniiFoundation.Services
{
    public class SocialService : ISocialService
    {
        #region Private Members
        private static ServiceHelpers serviceHelpers;
        #endregion

        #region Constructor
        public SocialService()
        {
            serviceHelpers = ServiceHelpers.Instance;
        }

        #endregion

        public async Task<List<FriendRequest>> GetFriendRequests()
        {
            List<FriendRequest> users = new List<FriendRequest>();
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetFriendRequestEndPoint, true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    users = JsonConvert.DeserializeObject<List<FriendRequest>>(response.Data);
                }
                else
                {
                    var msg = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Data);
                    await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, msg.Details == null ? DFResources.GenericErrorMessageText : msg.Details, DFResources.OKText);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return users;
        }

        public async Task<List<AppUser>> GetFriends()
        {
            List<AppUser> friends = new List<AppUser>();
            try
            {
                ResponseModel response = await serviceHelpers.GetRequestAsync(StringConstant.GetFriendsEndPoint, true, AppUtil.AccessToken);
                if (response.IsSuccess)
                {
                    friends = JsonConvert.DeserializeObject<List<AppUser>>(response.Data);
                }
                else
                {
                    var msg = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Data);
                    await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, msg.Details == null ? DFResources.GenericErrorMessageText : msg.Details, DFResources.OKText);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return friends;
        }

        public async Task<FriendRequest> SendFriendRequests(int userId, FriendRequest friendRequest)
        {
            FriendRequest requestResp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(friendRequest);
                ResponseModel response = await serviceHelpers.PostRequestAsync(serializedRequest, StringConstant.SendFriendRequestEndPoint.Replace("{user_id}", userId.ToString()), true, AppUtil.AccessToken);
                
                if (response.IsSuccess)
                {
                    requestResp = JsonConvert.DeserializeObject<FriendRequest>(response.Data);
                }
                else
                {
                    var msg = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Data);
                    await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, msg.Details == null ? DFResources.GenericErrorMessageText : msg.Details, DFResources.OKText);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return requestResp;
        }
        public async Task<FriendRequestUpdate> AcceptOrRejectFriendRequests(int friend_request_id, FriendRequestUpdate friendRequest)
        {
            FriendRequestUpdate requestResp = null;
            try
            {
                string serializedRequest = JsonConvert.SerializeObject(friendRequest);
                ResponseModel response = await serviceHelpers.PatchRequestAsync(serializedRequest, StringConstant.AcceptOrRejectFriendRequestEndPoint.Replace("{friend_request_id}", friend_request_id.ToString()), true, AppUtil.AccessToken);

                if (response.IsSuccess)
                {
                    requestResp = JsonConvert.DeserializeObject<FriendRequestUpdate>(response.Data);
                }
                else
                {
                    var msg = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Data);
                    await Application.Current.MainPage.DisplayAlert(DFResources.AlertText, msg.Details, DFResources.OKText);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return requestResp;
        }
    }
}
