using System;
using Tecnologia_vestible.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tecnologia_vestible
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new VistaPrincipal());
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
