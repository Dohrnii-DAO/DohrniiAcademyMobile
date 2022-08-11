using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.UserModels
{
    public class UserValidationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
