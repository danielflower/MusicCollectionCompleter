using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AlbumFinder.Desktop.Services;
using NUnit.Framework;

namespace AlbumFinder.AlbumFinderTests.Services
{
    
    public class SongFileFinderTest
    {
        internal static readonly DirectoryInfo dir = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleDir"));
        SongFileFinder finder = new SongFileFinder(dir);
        

        [Test]
        public void FindsAllSongsInADirRecursively()
        {
            var task = finder.FindSongsAsync(CancellationToken.None);
            Assert.IsTrue(task.Wait(TimeSpan.FromSeconds(20)));

            var songs = task.Result;
            CollectionAssert.AllItemsAreUnique(songs);
            Assert.AreEqual(5, songs.Count);
        }


        [Test]
        public void FindingFilesIsCancellable()
        {
            var source = new CancellationTokenSource();
            ManualResetEvent latch = new ManualResetEvent(false);
            source.Token.Register(() => latch.Set());
            
            var slowFinder = new SongFileFinder(new DirectoryInfo("C:\\"));
            var task = slowFinder.FindSongsAsync(source.Token);
            source.Cancel();

            Assert.IsTrue(latch.WaitOne(TimeSpan.FromSeconds(10)));
            Assert.IsTrue(task.IsCanceled);
        }
    }
}
