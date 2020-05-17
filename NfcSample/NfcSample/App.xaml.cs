using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace NfcSample {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart() {
            AppCenter.Start("android=bebb133a-7582-4d7d-9465-656b4a394660;", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
