using DohrniiFoundation.Models.Lessons;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models.UserModels
{
    [AddINotifyPropertyChangedInterface]
    public class UserStatus
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalXP { get; set; }
        public int TotalCryptoJelly { get; set; }
        public decimal TotalDHN { get; set; }
        public int XpPerCryptojelly { get; set; }
        public List<LessonInprogress> LessonsInprogress { get; set; }
    }
}
