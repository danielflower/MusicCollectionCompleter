using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCollectionCompleter.Desktop.Services;
using NUnit.Framework;

namespace MusicCollectionCompleter.MusicCollectionCompleterTests.Services
{
    public class AlbumIgonoranceDbTest
    {
        AlbumIgonoranceDb db = new AlbumIgonoranceDb(new DirectoryInfo(Path.Combine(Path.GetTempPath(), "TestData", Guid.NewGuid().ToString())));

        [Test]
        public void ReturnsAnEmptyListForAnUnknownAlbum()
        {
            CollectionAssert.IsEmpty(db.GetIgnoredAlbums("iamnotarealartist"));
        }

        [Test]
        public void AlbumsIgnorationCanBeStored()
        {
            var one = new Album("Album one");
            var two = new Album("Album two");
            var three = new Album("Album three");
            db.IgnoreAlbum("theartist", one);
            db.IgnoreAlbum("theartist", two);
            db.IgnoreAlbum("anotherartist", three);

            CollectionAssert.AreEquivalent(new[] { one, two }, db.GetIgnoredAlbums("theartist"));
        }

        [Test]
        public void ItCanHandleCrazyCharacters()
        {
            var crazy = "crazy: artist/\\!";
            CollectionAssert.IsEmpty(db.GetIgnoredAlbums(crazy));

            var one = new Album("The One");
            db.IgnoreAlbum(crazy, one);
            CollectionAssert.AreEquivalent(new[] { one }, db.GetIgnoredAlbums(crazy));

        }

    }
}
