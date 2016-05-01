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
            var songObservable = new Subject<Song>();
            var artistObserver = new Subject<Artist>();
            var actual = new List<string>();
            artistObserver.Subscribe(artist => actual.Add(artist.Name.ToLowerInvariant()));

            new AlbumDatabase(songObservable, artistObserver);
            Task.Factory.StartNew(() =>
            {
                songObservable.OnNext(SongTest.GetSong("Blink $potter\\Kanwella\\01 Prognosis.mp3"));
                songObservable.OnNext(SongTest.GetSong("Blink $potter\\Mutey & The Chuck\\01 Hart Start.mp3"));
                songObservable.OnNext(SongTest.GetSong("Blink $potter\\Mutey & The Chuck\\02 Monk Nonk.mp3"));
                songObservable.OnNext(
                    SongTest.GetSong("The Can't Notters\\Blove Grapes (special edition)\\Disc 1\\01 Blove Grapes!.mp3"));
                songObservable.OnNext(
                    SongTest.GetSong("The Can't Notters\\Blove Grapes (special edition)\\Disc 2\\01 Graphes Blove.mp3"));
                songObservable.OnCompleted();
            });
            artistObserver.Wait();

            CollectionAssert.AllItemsAreUnique(actual);
            CollectionAssert.AreEquivalent(new[] {"blink $potter", "the can't notters"}, actual);
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