using DohrniiFoundation.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DohrniiFoundation.Views.Lessons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChapterQuestionPage : ContentPage
    {
        public ChapterQuestionPage()
        {
            InitializeComponent();
        }

        private void COption_Tapped(object sender, EventArgs e)
        {
            try
            {
                var option = (ClassQuestionOptionModel)((Frame)sender).BindingContext;
                VM.OptionSelected(option);
            }
            catch (System.Exception)
            {

            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
    
}