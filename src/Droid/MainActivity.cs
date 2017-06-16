using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using GoodVibrations.Consts;
using GoodVibrations.Interfaces.Services;
using SQLite.Net.Interop;
using Splat;
using SQLite.Net.Platform.XamarinAndroid;
using KeyChain.Net.XamarinAndroid;
using KeyChain.Net;
using GoodVibrations.Shared;
using GoodVibrations.Services;

namespace GoodVibrations.Droid
{
    [Activity (Label = "GoodVibrations.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate (Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate (savedInstanceState);

            global::Xamarin.Forms.Forms.Init (this, savedInstanceState);

            RegisterPlatformServices();

            LoadApplication (new App ());
        }

        private void RegisterPlatformServices()
        {
            var resolver = Locator.CurrentMutable;

            resolver.Register(() => new SQLitePlatformAndroid(), typeof(ISQLitePlatform));

            resolver.RegisterLazySingleton(() => new KeyChainHelper(() => Application.Context, Constants.KeyChain.CommonKeyChainKeyStoreFileProtectionPassword), typeof(IKeyChainHelper));
        }
    }
}
