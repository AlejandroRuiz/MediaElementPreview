using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaElement
{
    public partial class App : Application
    {
        static App()
        {
            Device.SetFlags(new[] { "MediaElement_Experimental" });
        }

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
