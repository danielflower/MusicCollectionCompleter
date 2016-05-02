using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
            var artistObserver = new Subject<Artist>();
            var actual = new List<string>();
            ManualResetEvent latch = new ManualResetEvent(false);
            artistObserver.Subscribe(artist => actual.Add(artist.NormalisedName.ToLowerInvariant()), () => latch.Set());

            var db = new AlbumDatabase(artistObserver);
            db.ProcessSongAsync(SongTest.GetSong("Blink $potter\\Kanwella\\01 Prognosis.mp3"));
            db.ProcessSongAsync(SongTest.GetSong("Blink $potter\\Mutey & The Chuck\\01 Hart Start.mp3"));
            db.ProcessSongAsync(SongTest.GetSong("Blink $potter\\Mutey & The Chuck\\02 Monk Nonk.mp3"));
            db.ProcessSongAsync(SongTest.GetSong("The Can't Notters\\Blove Grapes (special edition)\\Disc 1\\01 Blove Grapes!.mp3"));
            db.ProcessSongAsync(SongTest.GetSong("The Can't Notters\\Blove Grapes (special edition)\\Disc 2\\01 Graphes Blove.mp3"));
            db.OnAllSongsAdded();
            Assert.IsTrue(latch.WaitOne(5000), "Timed out waiting for subscription completion");

            CollectionAssert.AllItemsAreUnique(actual);
            CollectionAssert.AreEquivalent(new[] {"blink $potter", "can't notters"}, actual);
        }

//        [Test]
//        public void DBBuildupIsCancellable()
//        {
//            var source = new CancellationTokenSource();
//            AlbumDatabase db = new AlbumDatabase();
//            var songFileFinder = new SongFileFinder(SongFileFinderTest.dir);
//            var songs = songFileFinder.FindSongsAsync(source.Token).Result;
//            var artists = new List<string>();
//
//            db.ArtistAdded += artist =>
//            {
//                artists.Add(artist.Name.ToLowerInvariant());
//                source.Cancel();
//            };
//
//            var task = db.AddSongs(songs, source.Token);
//            Assert.IsTrue(task.Wait(TimeSpan.FromSeconds(30)));
//
//            Assert.AreEqual(1, artists.Count);
//        }
    }
}