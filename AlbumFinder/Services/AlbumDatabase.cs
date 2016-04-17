using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlbumFinder.Desktop.Services
{
    internal class AlbumDatabase
    {
        private readonly Dictionary<string, Artist> _artists = new Dictionary<string, Artist>();
        public event ArtistAddedEventHandler ArtistAdded;
        public event AlbumAddedEventHandler AlbumAdded;

        public Task AddSongs(List<Song> songs, CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var song in songs)
                {
                    if (token.IsCancellationRequested)
                        return;
                    song.LoadInfo();
                    if (!string.IsNullOrEmpty(song.Artist) && !string.IsNullOrEmpty(song.Album))
                    {
                        AddArtistAndAlbumFor(song);
                    }
                }
            }, token);
        }

        private void AddArtistAndAlbumFor(Song song)
        {
            var artistName = Song.Normalise(song.Artist);
            if (!_artists.ContainsKey(artistName))
            {
                Artist artist = new Artist(song.Artist, artistName);
                _artists[artistName] = artist;
                ArtistAdded?.Invoke(artist);
            }
        }
    }

    public delegate void ArtistAddedEventHandler(Artist artist);

    public delegate void AlbumAddedEventHandler(Album artist);
}