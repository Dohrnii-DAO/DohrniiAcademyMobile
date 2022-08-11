using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.UserModels
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Region { get; set; }
        public string Device { get; set; }
    }
}
