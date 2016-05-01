using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using AlbumFinder.Desktop.Services;

namespace AlbumFinder.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyViewModel _viewModel;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new MyViewModel();
            DataContext = _viewModel;
        }
        

        private void FolderEntry_OnSelectedDirectoryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("Dir selected");

            DirectoryInfo dir = (DirectoryInfo) e.NewValue;
            SongFileFinder fileFinder = new SongFileFinder(dir);
            var cancel = new CancellationTokenSource();
//            var files = fileFinder.FindSongsAsync(cancel.Token);
//            var db = new AlbumDatabase();

//            db.ArtistAdded += a => _viewModel.Artists.Add(a);

//            db.AddSongs(files.Result, cancel.Token);


        }
    }

    public class MyViewModel
    {
        public List<Artist> Artists { get; }

        public MyViewModel()
        {
            Artists = new List<Artist>();
        }
    }
}
