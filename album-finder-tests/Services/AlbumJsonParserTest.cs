using System.Collections;
using System.Collections.Generic;
using System.IO;
using AlbumFinder.Desktop.Services;
using NUnit.Framework;

namespace AlbumFinder.AlbumFinderTests.Services
{
    public class AlbumJsonParserTest
    {

        [Test]
        public void FiltersOutRubbish()
        {
            var jsonFile = SongTest.GetFile("smashing-pumpkins.json");
            var json = File.ReadAllText(jsonFile.FullName);
            var albums = new AlbumJsonParser(5).ToAlbums(json);
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

        private static Album Album(string name, int year, long tunesCollectionId)
        {
            return new Album(name, year, tunesCollectionId);
        }

        [Test]
        public void ParenthesesAreRemovedFromAlbumTitles()
        {
            Assert.AreEqual("Pisces Iscariot", AlbumJsonParser.Normalise("Pisces Iscariot (Deluxe Edition)"));
            Assert.AreEqual("Pisces Iscariot", AlbumJsonParser.Normalise("The Pisces Iscariot (Deluxe Edition)"));
            Assert.AreEqual("Pisces Iscariot", AlbumJsonParser.Normalise("Pisces Iscariot"));
            Assert.AreEqual("Pisces Iscariot", AlbumJsonParser.Normalise("The Pisces Iscariot"));
            Assert.AreEqual("(Deluxe Edition)", AlbumJsonParser.Normalise("(Deluxe Edition)"));
        }
    }
}
