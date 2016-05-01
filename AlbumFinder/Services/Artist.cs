using System.Collections.Generic;

namespace AlbumFinder.Desktop.Services
{
    public class Artist
    {
        private readonly List<Album> _availableAlbums = new List<Album>();

        public Artist(string name, string normalisedName)
        {
            Name = name;
            NormalisedName = normalisedName;
        }

        public string Name { get; }
        public string NormalisedName { get; }

        public IList<Album> AvailableAlbums
        {
            get
            {
                lock (_availableAlbums)
                    return new List<Album>(_availableAlbums);
            }
        }

        public void AddAvailableAlbum(Album album)
        {
            lock (_availableAlbums)
                if (!_availableAlbums.Contains(album))
                    _availableAlbums.Add(album);
        }
    }
}