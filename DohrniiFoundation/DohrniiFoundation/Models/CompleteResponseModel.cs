using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace DohrniiFoundation.Models
{
    [AddINotifyPropertyChangedInterface]
    public class CompleteResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public int TotalXpEarned { get; set; }
        public int TotalJellyEarned { get; set; }
        public decimal TotalDhnEarned { get; set; }
        public double PercentageComplete { get; set; }
    }
}
