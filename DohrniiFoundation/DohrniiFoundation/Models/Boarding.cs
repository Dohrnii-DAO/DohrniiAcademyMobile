using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;


namespace DohrniiFoundation.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Boarding
    {
        public string Illustration { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
    }
}
