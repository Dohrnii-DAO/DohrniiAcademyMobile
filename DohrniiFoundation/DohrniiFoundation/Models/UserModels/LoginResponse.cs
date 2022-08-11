using DohrniiFoundation.Models.Socials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.UserModels
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiringDate { get; set; }
    }
}
