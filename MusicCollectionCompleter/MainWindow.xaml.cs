using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Threading;
using MusicCollectionCompleter.Desktop.Services;

namespace MusicCollectionCompleter.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public AppViewModel ViewModel = new AppViewModel();
        private AlbumIgonoranceDb _albumIgonoranceDb;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var collectionView = CollectionViewSource.GetDefaultView(ArtistsListBox.ItemsSource);
            collectionView.SortDescriptions.Clear();
            collectionView.SortDescriptions.Add(new SortDescription("NormalisedName", ListSortDirection.Ascending));
            collectionView.Refresh();

            _albumIgonoranceDb = new AlbumIgonoranceDb(
                new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MusicCollectionCompleter")));
        }

        private void FolderEntry_OnSelectedDirectoryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var path = (string)e.NewValue;
            if (!Directory.Exists(path))
            {
                MessageBox.Show(this, "The directory " + path + " does not exist.", "Sorry...", MessageBoxButton.OK);
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            SongFileFinder fileFinder = new SongFileFinder(dir);

            var songObserver = new Subject<Song>();
            var albumLookerUpperer = new AlbumLookerUpperer();
            


            var artistObserver = new Subject<Artist>();
            artistObserver.Subscribe(artist =>
            {
                albumLookerUpperer.ProcessArtistAsync(artist);
                OnGuiThread(() => ViewModel.Artists.Add(artist));
                foreach (var ignoredAlbum in _albumIgonoranceDb.GetIgnoredAlbums(artist.NormalisedName))
                {
                    artist.IgnoreAlbum(ignoredAlbum);
                }
            });


            var db = new AlbumDatabase(artistObserver);
            songObserver.Subscribe(db.ProcessSongAsync, db.OnAllSongsAdded);
            
            fileFinder.FindSongsAsync(songObserver);
        }

        internal static void OnGuiThread(Action target)
        {
            Application.Current?.Dispatcher.Invoke(DispatcherPriority.Render, new Action(target));
        }

        private void MissingAlbumsArtistsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
            {
                return;
            }
            var addedItem = (Artist)e.AddedItems[0];
            MissingAlbumsPanel.DataContext = addedItem;
            MissingAlbumsScrollViewer.ScrollToTop();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            var artist = MissingAlbumsPanel.DataContext as Artist;
            if (artist != null)
            {
                var album = (Album) ((Hyperlink) sender).DataContext;
                artist.IgnoreAlbum(album);
                _albumIgonoranceDb.IgnoreAlbum(artist.NormalisedName, album);
            }
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
