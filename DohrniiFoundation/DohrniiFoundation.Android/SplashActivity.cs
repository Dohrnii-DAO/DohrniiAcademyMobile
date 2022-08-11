using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;

namespace DohrniiFoundation.Droid
{
    [Activity(Label = "Dohrnii Academy", Icon = "@mipmap/Icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }
        protected override void OnResume()
        {
            base.OnResume();
            //System.Threading.Thread.Sleep(100);
            //StartActivity(typeof(MainActivity));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}