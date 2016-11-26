
using Foundation;
using KeyChain.Net;
using KeyChain.Net.XamarinIOS;
using Security;
using Splat;
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

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private void RegisterPlatformServices()
        {
            var resolver = Locator.CurrentMutable;
            resolver.RegisterLazySingleton(() => new KeyChainHelper(Constants.KeyChain.CommonKeyChainNamespace, false, SecAccessible.Always), typeof(IKeyChainHelper));
        }
    }
}
