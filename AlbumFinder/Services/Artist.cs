using System;
using System.Collections.Generic;
using System.Linq;

namespace AlbumFinder.Desktop.Services
{
    public class Artist
    {
        private readonly List<Album> _availableAlbums = new List<Album>();
        private readonly List<Album> _ownedAlbums = new List<Album>();
        private readonly List<Album> _missingAlbums = new List<Album>();

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
            lock (_availableAlbums)
                lock (_missingAlbums)
                {
                    _missingAlbums.Clear();
                    foreach (var album in _availableAlbums)
                    {
                        var b = from owned in _ownedAlbums
                            where String.Equals(owned.NormalisedName, album.NormalisedName, StringComparison.InvariantCultureIgnoreCase)
                            select owned;
                        if (!b.Any())
                        {
                            _missingAlbums.Add(album);
                        }
                    }
                }
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
    }
}