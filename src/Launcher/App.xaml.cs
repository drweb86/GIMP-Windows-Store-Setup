using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Launcher
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync().AsTask().Wait();
            }
            else
            {
                Current.Exit();
            }
        }
    }
}
