﻿using System;
using System.IO;
using AlbumFinder.Desktop.Services;
using NUnit.Framework;

namespace AlbumFinder.AlbumFinderTests.Services
{
    public class SongTest
    {
        [Test]
        public void CanLoadTagInfo()
        {
            Song bloveGrapes = GetSong("The Can't Notters\\Blove Grapes (special edition)\\Disc 1\\01 Blove Grapes!.mp3");
            bloveGrapes.LoadInfo();
            Assert.AreEqual("The Can't Notters", bloveGrapes.Artist);
            Assert.AreEqual("Blove Grapes (special edition)", bloveGrapes.Album);
        }

        private static Song GetSong(string path)
        {
            var file = new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleDir", path));
            if (!file.Exists)
                throw new Exception("Could not find " + file.FullName);

            return new Song(file);
        }

        [Test]
        public void NormalisationIgnoresCaseAndArticles()
        {
            string[] pumpkins = {"The Smashing Pumpkins", "Smashing Pumpkins", "smashing pumpkins", "Smashing pumpkins "};
            foreach (var name in pumpkins)
            {
                Assert.AreEqual("smashing pumpkins", Song.Normalise(name));
            }
        }

        [Test]
        public void NormalisationReturnsOriginalTrimmedStringIfAllBitsAreStripped()
        {
            string[] pumpkins = {"The ", "The", "the", " the "};
            foreach (var name in pumpkins)
            {
                Assert.AreEqual("the", Song.Normalise(name));
            }
        }
    }
}
