using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Threading;
using AlbumFinder.Desktop.Services;

namespace AlbumFinder.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public AppViewModel ViewModel = new AppViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }


        private void FolderEntry_OnSelectedDirectoryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo((string)e.NewValue);
            SongFileFinder fileFinder = new SongFileFinder(dir);

            var songObserver = new Subject<Song>();


            var artistObserver = new Subject<Artist>();
            artistObserver.Subscribe(artist =>
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => ViewModel.Artists.Add(artist)));
            });


            var db = new AlbumDatabase(songObserver, artistObserver);

            fileFinder.FindSongsAsync(songObserver);
        }

    }

    public class AppViewModel
    {
        public ObservableCollection<Artist> Artists { get; }

        public AppViewModel()
        {
            Artists = new ObservableCollection<Artist>();
        }
    }
    
}
