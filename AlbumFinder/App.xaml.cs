using System.Windows;
using AlbumFinder.Desktop.Properties;

namespace AlbumFinder.Desktop
{
    public partial class App : Application
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
