using GoodVibrations.Pages;
using GoodVibrations.ViewModels;
using Xamarin.Forms;

namespace GoodVibrations
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent ();

            RegisterViewModels();

            MainPage = new NavigationPage(new MainPage ());
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }

        private void RegisterViewModels()
        {
            var resolver = Splat.Locator.CurrentMutable;

            resolver.Register(() => new MainViewModel(), typeof(MainViewModel));
            resolver.Register(() => new EditNotificatorViewModel(), typeof(EditNotificatorViewModel));
            resolver.Register(() => new LoginViewModel(), typeof(LoginViewModel));
            resolver.Register(() => new PhoneCallTemplateViewModel(), typeof(PhoneCallTemplateViewModel));
        }
    }
}
