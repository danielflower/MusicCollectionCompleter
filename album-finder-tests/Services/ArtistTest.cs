using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlbumFinder.Desktop.Services;
using NUnit.Framework;

namespace AlbumFinder.AlbumFinderTests.Services
{
    public class ArtistTest
    {
        [Test]
        public void CanIdentifyMissingAlbums()
        {
            Artist artist = new Artist("Blur", "blur");
            var modernLifeIsRubbish = new Album("Modern Life is Rubbish", 1992, 892374589, "http://blah/blah", "http://blah/blah.jpg");
            artist.AddAvailableAlbums(new[] {new Album("Blur", 1999, 12341234, "http://blah/blah", "http://blah/blah.jpg"), modernLifeIsRubbish});
            artist.AddOwnedAlbum(new Album("the blur"));
            artist.AddOwnedAlbum(new Album("Great Escape"));

            CollectionAssert.AreEquivalent(new[] {modernLifeIsRubbish}, artist.MissingAlbums);
        }
    }
}