using Newtonsoft.Json;

namespace DohrniiFoundation.Models.APIRequestModel.User
{
    /// <summary>
    /// This model class is used to send the request to change the password
    /// </summary>
   public class ChangePasswordRequestModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
