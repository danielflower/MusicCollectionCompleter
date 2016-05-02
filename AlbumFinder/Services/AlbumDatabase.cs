using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlbumFinder.Desktop.Services
{
    internal class AlbumDatabase
    {
        private readonly IObserver<Artist> _artistObserver;
        private readonly Dictionary<string, Artist> _artists = new Dictionary<string, Artist>();
        private readonly ConcurrentQueue<Task> _runningTasks = new ConcurrentQueue<Task>();

        public AlbumDatabase(IObserver<Artist> artistObserver)
        {
            _artistObserver = artistObserver;
        }
        

        public void ProcessSongAsync(Song song)
        {
            var task = Task.Factory.StartNew(() =>
            {
                song.LoadInfo();
                if (!string.IsNullOrEmpty(song.Artist) && !string.IsNullOrEmpty(song.Album))
                {
                    AddArtistAndAlbumFor(song);
                }
            });
            _runningTasks.Enqueue(task);
        }

        private void AddArtistAndAlbumFor(Song song)
        {
            var artistName = Artist.Normalise(song.Artist);
            lock (_artists)
            {
                Artist artist;
                
                if (_artists.ContainsKey(artistName))
                {
                    artist = _artists[artistName];
                }
                else
                {
                    artist = new Artist(song.Artist, artistName);
                    _artists[artistName] = artist;
                    _artistObserver.OnNext(artist);
                }
                artist.AddOwnedAlbum(new Album(song.Album));
                
            }
        }

        public void OnAllSongsAdded()
        {
            Task.WaitAll(_runningTasks.ToArray());
            _artistObserver.OnCompleted();
        }
    }
}