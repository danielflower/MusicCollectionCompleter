using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace AlbumFinder.Desktop.Services
{
    public class Artist
    {
        private readonly List<Album> _availableAlbums = new List<Album>();
        private readonly ObservableCollection<Album> _missingAlbums = new ObservableCollection<Album>();

        public Artist(string name, string normalisedName)
        {
            Name = name;
            NormalisedName = normalisedName;
        }

        public string Name { get; }
        public string NormalisedName { get; }

        public ObservableCollection<Album> MissingAlbums => _missingAlbums;

        public IList<Album> AvailableAlbums
        {
            get
            {
                lock (_availableAlbums)
                    return new List<Album>(_availableAlbums);
            }
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
            lock (_availableAlbums)
                lock (_missingAlbums)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
                    {
                        foreach (var album in _availableAlbums)
                        {
                            if (!_missingAlbums.Contains(album))
                            {
                                _missingAlbums.Add(album);
                            }
                        }
                    }));
                }
        }
    }
}