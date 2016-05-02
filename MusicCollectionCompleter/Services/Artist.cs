using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private readonly List<Album> _ownedAlbums = new List<Album>();
        private readonly List<Album> _missingAlbums = new List<Album>();
        private Color _backgroundColor = Loading;

        public Artist() : this(null, null) { } // For XAML support

        public Artist(string name, string normalisedName)
        {
            Name = name;
            NormalisedName = normalisedName;
        }

        public string Name { get; }
        public string NormalisedName { get; }

        public List<Album> MissingAlbums => _missingAlbums;

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
            set {
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
                if (!_ownedAlbums.Contains(album))
                    _ownedAlbums.Add(album);
            UpdateMissingAlbums();
        }

        public void AddAvailableAlbums(IEnumerable<Album> albums)
        {
            lock (_availableAlbums)
                foreach (var album in albums)
                    if (!_availableAlbums.Contains(album))
                        _availableAlbums.Add(album);
            UpdateMissingAlbums();
        }

        private void UpdateMissingAlbums()
        {
            Color newColor = Loading;
            lock (_availableAlbums)
                lock (_missingAlbums)
                {
                    _missingAlbums.Clear();
                    foreach (var album in _availableAlbums)
                    {
                        var b = from owned in _ownedAlbums
                            where string.Equals(owned.NormalisedName, album.NormalisedName)
                            select owned;
                        if (!b.Any())
                        {
                            _missingAlbums.Add(album);
                        }
                    }
                    newColor = (_missingAlbums.Count == 0) ? Complete : Missing;
                }
            BackgroundColor = newColor;
            OnPropertyChanged(nameof(MissingAlbums));

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
    }
}