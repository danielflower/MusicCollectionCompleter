using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using MusicCollectionCompleter.Desktop.Services;
using NUnit.Framework;

namespace MusicCollectionCompleter.MusicCollectionCompleterTests.Services
{
    public class SongFileFinderTest
    {
        internal static readonly DirectoryInfo Dir =
            new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleDir"));

        readonly SongFileFinder _finder = new SongFileFinder(Dir);
        readonly Subject<Song> _songObserver = new Subject<Song>();
        readonly List<Song> _actual = new List<Song>();

        [Test]
        public void FindsAllSongsInADirRecursively()
        {
            _songObserver.Subscribe(song => _actual.Add(song));
            _finder.FindSongsAsync(_songObserver);
            _songObserver.Wait();

            CollectionAssert.AllItemsAreUnique(_actual);
            Assert.AreEqual(5, _actual.Count);
        }


//        [Test]
//        public void FindingFilesIsCancellable()
//        {
//            var source = new CancellationTokenSource();
//            ManualResetEvent latch = new ManualResetEvent(false);
//            source.Token.Register(() => latch.Set());
//            
//            var slowFinder = new SongFileFinder(new DirectoryInfo("C:\\"));
//            var task = slowFinder.FindSongsAsync(source.Token);
//            source.Cancel();
//
//            Assert.IsTrue(latch.WaitOne(TimeSpan.FromSeconds(10)));
//            Assert.IsTrue(task.IsCanceled);
//        }
    }
}