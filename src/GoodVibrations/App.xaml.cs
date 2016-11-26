using Xamarin.Forms;

namespace GoodVibrations
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent ();

            MainPage = new GoodVibrationsPage ();
			var resolver = Splat.Locator.CurrentMutable;

			//var persistenceService = resolver.Register<IUserSerive>();
			//persistenceService.Initialize();
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
    }
}
