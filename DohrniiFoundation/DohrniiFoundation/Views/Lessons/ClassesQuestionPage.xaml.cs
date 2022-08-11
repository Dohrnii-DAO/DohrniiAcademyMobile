using DohrniiFoundation.Models.Lessons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DohrniiFoundation.Views.Lessons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassesQuestionPage : ContentPage
    {
        public ClassesQuestionPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void COption_Tapped(object sender, System.EventArgs e)
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
    }
}