using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DohrniiFoundation.Enum;
using DohrniiFoundation.Helpers;
using DohrniiFoundation.IServices;
using DohrniiFoundation.Models.Socials;
using DohrniiFoundation.Services;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace DohrniiFoundation.ViewModels.Socials
{
	public class PendingRequestViewModel: BaseViewModel
	{
        private static ISocialService socialService;

        #region Props
        public List<FriendRequest> FriendRequests { get; set; }
        #endregion
        public PendingRequestViewModel()
        {
            socialService = DependencyService.Get<SocialService>(); //new SocialService();
        }
        public async Task InitData()
        {
            try
            {
                IsLoading = true;
                var requests = await socialService.GetFriendRequests();
                FriendRequests = requests.Where(c=>c.Receiver.Id == AppUtil.CurrentAppUser.Id & c.Status == FriendRequestStatus.P.ToString()).ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task AcceptOrRejectFriendRequest(int requestId, FriendRequestStatus requestStatus, FriendRequest request)
        {
            try
            {
                IsLoading = true;
                var result = await socialService.AcceptOrRejectFriendRequests(requestId, new FriendRequestUpdate { status = requestStatus.ToString()});
                if(result.status == FriendRequestStatus.A.ToString())
                {
                    request.Status = FriendRequestStatus.A.ToString();
                }
                else if(result.status == FriendRequestStatus.R.ToString())
                {
                    request.Status = FriendRequestStatus.R.ToString();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}

