using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AudioRecorderV4
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            var appFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var binFolder = System.IO.Path.Combine(appFolder, "bin");

            Process.Start(new ProcessStartInfo() { CreateNoWindow=true, UseShellExecute=true, FileName= "cmd", Arguments = $"/c \"start /b gimp-{version.Major}.{version.Minor}.exe\"",  WorkingDirectory = binFolder });
        }
    }
}
