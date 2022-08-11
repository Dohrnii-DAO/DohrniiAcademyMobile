using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models
{
    [AddINotifyPropertyChangedInterface]
    public class StartResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStarted { get; set; }
    }
}
