using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;
using MusicCollectionCompleter.Desktop.Properties;

namespace MusicCollectionCompleter.Desktop.Services
{
    public class Artist : INotifyPropertyChanged
    {
        private static readonly Color Loading = Colors.Transparent;
        private static readonly Color Missing = Colors.OrangeRed;
        private static readonly Color Complete = Colors.LimeGreen;

        private readonly List<Album> _availableAlbums = new List<Album>();
        private volatile bool _availableAlbumsKnown = false;
        private readonly List<Album> _ownedAlbums = new List<Album>();
        private readonly AlbumList _missingAlbums = new AlbumList();
        private Color _backgroundColor = Loading;

        public Artist() : this(null, null)
        {
        } // For XAML support

        public Artist(string name, string normalisedName)
        {
            Name = name;
            NormalisedName = normalisedName;
            BindingOperations.EnableCollectionSynchronization(_missingAlbums, _missingAlbums);
        }

        public string Name { get; }
        public string NormalisedName { get; }

        public AlbumList MissingAlbums
        {
            get { return _missingAlbums; }
        }

        public IList<Album> AvailableAlbums
        {
            get
            {
                lock (_availableAlbums)
                    return new List<Album>(_availableAlbums);
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (!Equals(value, _backgroundColor))
                {
                    _backgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public void AddOwnedAlbum(Album album)
        {
            lock (_ownedAlbums)
                lock (_missingAlbums)
                {
                    if (!_ownedAlbums.Contains(album))
                    {
                        _ownedAlbums.Add(album);
                    }
                    _missingAlbums.Remove(album);
                }

            UpdateBackgroundColor();
        }

        public void AddAvailableAlbums(IEnumerable<Album> albums)
        {
            lock (_ownedAlbums)
                lock (_missingAlbums)
                    lock (_availableAlbums)
                    {
                        foreach (var album in albums)
                        {
                            if (!_availableAlbums.Contains(album))
                                _availableAlbums.Add(album);
                            if (!_ownedAlbums.Contains(album))
                                _missingAlbums.Add(album);
                        }
                        _availableAlbumsKnown = true;
                    }
            UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor()
        {
            Color newColor = Loading;
            if (_availableAlbumsKnown)
            {
                lock (_missingAlbums)
                {
                    newColor = (_missingAlbums.Count == 0) ? Complete : Missing;
                }
            }
            BackgroundColor = newColor;
        }

        public static string Normalise(string artist)
        {
            artist = artist.ToLowerInvariant().Trim();
            if (artist.StartsWith("the "))
                artist = artist.Substring(4);
            if (artist.StartsWith("a "))
                artist = artist.Substring(2);
            artist = artist.Replace("&", "and");

            return artist;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IgnoreAlbum(Album album)
        {
            AddOwnedAlbum(album);
            UpdateBackgroundColor();
        }
    }
}