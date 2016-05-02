using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Data;
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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var collectionView = CollectionViewSource.GetDefaultView(ArtistsListBox.ItemsSource);
            collectionView.SortDescriptions.Clear();
            collectionView.SortDescriptions.Add(new SortDescription("NormalisedName", ListSortDirection.Ascending));
            collectionView.Refresh();
        }

        private void FolderEntry_OnSelectedDirectoryChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo((string)e.NewValue);
            SongFileFinder fileFinder = new SongFileFinder(dir);

            var songObserver = new Subject<Song>();
            var albumLookerUpperer = new AlbumLookerUpperer();


            var artistObserver = new Subject<Artist>();
            artistObserver.Subscribe(artist =>
            {
                albumLookerUpperer.ProcessArtistAsync(artist);
                OnGuiThread(() =>
                {
                    ViewModel.Artists.Add(artist);
                });
            });


            var db = new AlbumDatabase(artistObserver);
            songObserver.Subscribe(db.ProcessSongAsync, db.OnAllSongsAdded);
            
            fileFinder.FindSongsAsync(songObserver);
        }

        internal static void OnGuiThread(Action target)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(target));
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
