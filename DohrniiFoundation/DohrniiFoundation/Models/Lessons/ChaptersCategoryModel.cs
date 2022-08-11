using DohrniiFoundation.Helpers;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace DohrniiFoundation.Models.Lessons
{
    [AddINotifyPropertyChangedInterface]
    public class ChaptersCategoryModel : BaseViewModel
    {
        #region Properties
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
        public bool IsSelected { get; set; }
        public string BGColor
        {
            get
            {
                if (IsSelected)
                {
                    return StringConstant.BorderColor2;
                }
                else
                {
                    return StringConstant.Transparent;
                }
            }
        }



        private bool isNotGrdientVisible;
        public bool IsNotGrdientVisible
        {
            get { return isNotGrdientVisible; }
            set
            {
                if (isNotGrdientVisible != value)
                {
                    isNotGrdientVisible = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool isGradientVisible;
        public bool IsGradientVisible
        {
            get { return isGradientVisible; }
            set
            {
                if (isGradientVisible != value)
                {
                    isGradientVisible = value;
                    this.OnPropertyChanged();
                }
            }
        }
        private bool isCategorySelected;
        public bool IsCategorySelected
        {
            get { return isCategorySelected; }
            set
            {
                if (isCategorySelected != value)
                {
                    isCategorySelected = value;
                    this.OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
