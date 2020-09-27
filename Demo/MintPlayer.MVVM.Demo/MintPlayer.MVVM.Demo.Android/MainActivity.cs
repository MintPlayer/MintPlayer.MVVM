﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using MintPlayer.MVVM.Platforms.Common;

namespace MintPlayer.MVVM.Demo.Droid
{
    [Activity(Label = "MintPlayer.MVVM.Demo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private MintPlayerMvvmConfiguration mvvmConfiguration;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            mvvmConfiguration = Platforms.Android.Platform.Init<App, Startup>(this);
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            var handled = mvvmConfiguration.BackButtonPressedHandler();
            if (!handled)
            {
                base.OnBackPressed();
            }
        }
    }
}