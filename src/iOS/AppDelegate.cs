
using Foundation;
using GoodVibrations.Consts;
using GoodVibrations.Interfaces.Services;
using GoodVibrations.iOS.Services;
using GoodVibrations.Shared;
using KeyChain.Net;
using KeyChain.Net.XamarinIOS;
using Plugin.Toasts;
using Security;
using Splat;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace GoodVibrations.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// Ask the user for permission to get notifications on iOS 10.0+
				UNUserNotificationCenter.Current.RequestAuthorization(
					UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
					(approved, error) => { });

				// Watch for notifications while app is active
				UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
			}
			else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				// Ask the user for permission to get notifications on iOS 8.0+
				var settings = UIUserNotificationSettings.GetSettingsForTypes(
					UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

            Xamarin.Forms.Forms.Init();

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init();

            RegisterPlatformServices();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private void RegisterPlatformServices()
        {
            var resolver = Locator.CurrentMutable;
			resolver.Register(() => new SQLitePlatformIOS(), typeof(ISQLitePlatform));
            resolver.RegisterLazySingleton(() => new KeyChainHelper(Constants.KeyChain.CommonKeyChainNamespace, false, SecAccessible.Always), typeof(IKeyChainHelper));
            resolver.Register (() => new ImageService (), typeof (IImageService));

        }
    }
}
