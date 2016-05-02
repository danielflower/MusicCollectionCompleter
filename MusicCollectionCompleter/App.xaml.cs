using System.Windows;
using MusicCollectionCompleter.Desktop.Properties;

namespace MusicCollectionCompleter.Desktop
{
    public partial class App : Application
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
