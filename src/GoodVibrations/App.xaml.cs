using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Pages;
using GoodVibrations.Services;
using GoodVibrations.ViewModels;
using KeyChain.Net;
using Splat;
using SQLite.Net.Interop;
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

            var persistence = Locator.Current.GetService<IPersistenceService>();
            persistence.Initialize();

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

            resolver.Register(() => new MainViewModel(resolver.GetService<IPersistenceService>()), typeof(MainViewModel));
            resolver.Register(() => new RegistrationViewModel(resolver.GetService<IKeyChainHelper>(), resolver.GetService<IAuthentificationSerivce> ()), typeof(RegistrationViewModel));
            resolver.Register(() => new EditNotificatorViewModel(resolver.GetService<IPersistenceService>()), typeof(EditNotificatorViewModel));
            resolver.Register(() => new LoginViewModel(resolver.GetService<IKeyChainHelper>(), resolver.GetService<IAuthentificationSerivce> ()), typeof(LoginViewModel));
            resolver.Register(() => new PhoneCallTemplateViewModel(resolver.GetService<IPersistenceService>()), typeof(PhoneCallTemplateViewModel));

			resolver.RegisterLazySingleton(() => new NotificationService(resolver.GetService<IPersistenceService> ()), typeof(INotificationService));
			resolver.RegisterLazySingleton(() => new PersistenceService(resolver.GetService<ISQLitePlatform>()), typeof(IPersistenceService));
			resolver.RegisterLazySingleton(() => new PhoneCallService(), typeof(IPhoneCallService));
			resolver.RegisterLazySingleton(() => new AuthentificationSerivce(), typeof(IAuthentificationSerivce));
		}
    }
}
