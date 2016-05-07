using System.IO;
using MusicCollectionCompleter.Desktop.Services;
using NUnit.Framework;

namespace MusicCollectionCompleter.MusicCollectionCompleterTests.Services
{
    public class AlbumJsonParserTest
    {

        [Test]
        public void FiltersOutRubbish()
        {
            var json = Json("smashing-pumpkins.json");
            var albums = new AlbumJsonParser(5).ToAlbums("smashing pumpkins", json);
            Album[] expected = {
                Album("Machina - The Machines of God", 2000, 712756036),
                Album("Greatest Hits", 2001, 712732565),
                Album("Rarities and B-Sides", 2005, 724942437),
                Album("Gish", 2012, 721209887),
                Album("Mellon Collie and the Infinite Sadness", 2012, 721224313),
                Album("Oceania", 2012, 720200110),
                Album("Pisces Iscariot", 2012, 721242180),
                Album("Siamese Dream", 2012, 721207206),
                Album("Aeroplane Flies High", 2013, 673231276),
                Album("Adore", 2014, 918891054),
                Album("Monuments to an Elegy", 2014, 929790535),
            };
            CollectionAssert.AreEqual(expected, albums);
        }

        [Test]
        public void FiltersOutNonRelatedBands()
        {
            var json = Json("Air.json");
            var albums = new AlbumJsonParser(4).ToAlbums("air", json);
            Album[] expected = {
                Album("Moon Safari", 1998, 693063670),
                Album("Premiers Symptômes - EP", 1999, 966652812),
                Album("Playground Love - EP", 2000, 696809600),
                Album("10 000 Hz Legend", 2001, 695721907),
                Album("Talkie Walkie", 2004, 696641105),
                Album("Pocket Symphony", 2007, 716499623),
                Album("Moon Safari Remixes, Rarities and Radio Sessions", 2008, 700013433),
                Album("Love 2", 2009, 693074986),
                Album("Le Voyage Dans La Lune", 2012, 693075451),
                Album("Virgin Suicides", 2015, 998794459),
                Album("Twentyears", 2016, 1103303922),
            };
            CollectionAssert.AreEqual(expected, albums);
        }

        private static string Json(string filename)
        {
            var jsonFile = SongTest.GetFile("iTunes JSON\\" + filename);
            var json = File.ReadAllText(jsonFile.FullName);
            return json;
        }

        private static Album Album(string name, int year, long tunesCollectionId)
        {
            return new Album(name, year, tunesCollectionId, "http://blah/blah", "http://blah/blah.png");
        }

    }
}
