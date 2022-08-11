using DohrniiFoundation.Enum;
using DohrniiFoundation.Models.Socials;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DohrniiFoundation.Views.Socials
{	
	public partial class PendingRequestPage : ContentPage
	{	
		public PendingRequestPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            await pendingRequestVM.InitData();
        }

        private async void Accept_Tapped(object sender, EventArgs e)
        {
            var request = (FriendRequest)(sender as Frame).BindingContext;
            await pendingRequestVM.AcceptOrRejectFriendRequest(request.Id, FriendRequestStatus.A, request);
        }

        private async void Reject_Tapped(object sender, EventArgs e)
        {
            var request = (FriendRequest)(sender as Frame).BindingContext;
            await pendingRequestVM.AcceptOrRejectFriendRequest(request.Id, FriendRequestStatus.R, request);
        }
    }
}

