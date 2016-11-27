
using Foundation;
using GoodVibrations.Consts;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.Shared;
using KeyChain.Net;
using KeyChain.Net.XamarinIOS;
using Security;
using Splat;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using UIKit;

namespace GoodVibrations.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Xamarin.Forms.Forms.Init();

            RegisterPlatformServices();

            LoadApplication(new App());

            var ss = new MicrosoftBandService ();
            ss.ConnectAndReadData ().ConfigureAwait (false);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private void RegisterPlatformServices()
        {
            var resolver = Locator.CurrentMutable;

			resolver.Register(() => new SQLitePlatformIOS(), typeof(ISQLitePlatform));
            resolver.RegisterLazySingleton (() => new MicrosoftBandService (), typeof (IMicrosoftBandService));
            resolver.RegisterLazySingleton(() => new KeyChainHelper(Constants.KeyChain.CommonKeyChainNamespace, false, SecAccessible.Always), typeof(IKeyChainHelper));
			resolver.RegisterLazySingleton(() => new MicrosoftBandService(), typeof(IMicrosoftBandService));
        }
    }
}
