using System.Threading.Tasks;
using GoodVibrations.Pages;
using GoodVibrations.ViewModels;
using KeyChain.Net;
using Splat;
using Xamarin.Forms;

namespace GoodVibrations
{
    public partial class App : Application
    {
        public App ()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) => 
                System.Diagnostics.Debug.WriteLine($"Unhandled Error: {e.Exception}");

            InitializeComponent ();

            RegisterViewModels();

            MainPage = new NavigationPage(new LoginPage ());
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
            var resolver = Locator.CurrentMutable;

            resolver.Register(() => new MainViewModel(), typeof(MainViewModel));
            resolver.Register(() => new RegistrationViewModel(resolver.GetService<IKeyChainHelper>()), typeof(RegistrationViewModel));
            resolver.Register(() => new EditNotificatorViewModel(), typeof(EditNotificatorViewModel));
            resolver.Register(() => new LoginViewModel(resolver.GetService<IKeyChainHelper>()), typeof(LoginViewModel));
            resolver.Register(() => new PhoneCallTemplateViewModel(), typeof(PhoneCallTemplateViewModel));
        }
    }
}
