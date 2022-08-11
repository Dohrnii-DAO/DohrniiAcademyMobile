using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.UserModels
{
    [AddINotifyPropertyChangedInterface]
    public class ChangePasswordResp
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
