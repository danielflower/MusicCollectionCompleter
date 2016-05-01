using System;
using System.Collections.Generic;

namespace AlbumFinder.Desktop.Services
{
    internal class AlbumDatabase
    {
        private readonly IObserver<Artist> _artistObserver;
        private readonly Dictionary<string, Artist> _artists = new Dictionary<string, Artist>();

        public AlbumDatabase(IObservable<Song> songObservable, IObserver<Artist> artistObserver)
        {
            _artistObserver = artistObserver;
            songObservable.Subscribe(OnNext, artistObserver.OnCompleted);
        }
        

        private void OnNext(Song song)
        {
            song.LoadInfo();
            if (!string.IsNullOrEmpty(song.Artist) && !string.IsNullOrEmpty(song.Album))
            {
                AddArtistAndAlbumFor(song);
            }
        }

        private void AddArtistAndAlbumFor(Song song)
        {
            var artistName = Song.Normalise(song.Artist);
            if (!_artists.ContainsKey(artistName))
            {
                Artist artist = new Artist(song.Artist, artistName);
                _artists[artistName] = artist;
                _artistObserver.OnNext(artist);
            }
        }
    }
}