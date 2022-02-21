using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.Storage;

namespace Wrapper
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync().AsTask().Wait(); ;
            }
        }
    }
}
