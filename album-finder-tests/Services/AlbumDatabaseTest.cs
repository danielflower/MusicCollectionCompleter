using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlbumFinder.Desktop.Services;
using NUnit.Framework;

namespace AlbumFinder.AlbumFinderTests.Services
{
    public class AlbumDatabaseTest
    {

        [Test]
        public void RaisesEventsWhenThingsAreDetected()
        {
            var source = new CancellationTokenSource();
            AlbumDatabase db = new AlbumDatabase();
            var songFileFinder = new SongFileFinder(SongFileFinderTest.dir);
            var songs = songFileFinder.FindSongsAsync(source.Token).Result;
            var artists = new List<string>();

            db.ArtistAdded += artist => artists.Add(artist.Name.ToLowerInvariant());

            var task = db.AddSongs(songs, source.Token);
            Assert.IsTrue(task.Wait(TimeSpan.FromSeconds(30)));

            CollectionAssert.AllItemsAreUnique(artists);
            CollectionAssert.AreEquivalent(new[] {"blink $potter", "the can't notters"}, artists);
        }

        [Test]
        public void DBBuildupIsCancellable()
        {
            var source = new CancellationTokenSource();
            AlbumDatabase db = new AlbumDatabase();
            var songFileFinder = new SongFileFinder(SongFileFinderTest.dir);
            var songs = songFileFinder.FindSongsAsync(source.Token).Result;
            var artists = new List<string>();

            db.ArtistAdded += artist =>
            {
                artists.Add(artist.Name.ToLowerInvariant());
                source.Cancel();
            };

            var task = db.AddSongs(songs, source.Token);
            Assert.IsTrue(task.Wait(TimeSpan.FromSeconds(30)));

            Assert.AreEqual(1, artists.Count);
        }

    }
}
