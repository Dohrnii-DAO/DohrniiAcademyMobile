using DohrniiFoundation.Resources;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.UserModels
{
    [AddINotifyPropertyChangedInterface]
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string WalletAddress { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateAdded { get; set; }
        public int TotalXp { get; set; }
        public int TotalJelly { get; set; }
        public decimal TotalDhn { get; set; }

        public string DisplayAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(WalletAddress))
                {
                    if (WalletAddress.Length > 15)
                    {
                        return WalletAddress.Substring(0, 8) + new string('.', (WalletAddress.Length - 8) - 4) + WalletAddress.Substring(WalletAddress.Length - 4);
                    }
                }
                return DFResources.ConnectWalletText;
            }
        }
    }
}
