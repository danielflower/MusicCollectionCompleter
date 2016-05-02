﻿using System;
using AlbumFinder.Desktop.Services;
using NUnit.Framework;

namespace AlbumFinder.AlbumFinderTests.Services
{
    public class AlbumLookerUppererTest
    {
        private readonly AlbumLookerUpperer _looker = new AlbumLookerUpperer();

        [Test]
        public void CanCallITunesAndGetAJsonResponse()
        {
            var json = _looker.GetJsonAsync("Smashing Pumpkins");
            Assert.IsTrue(json.Wait(TimeSpan.FromSeconds(60)), "Timed out calling itunes");
            Console.WriteLine(json.Result);
        }


        [OneTimeTearDown]
        public void DisposeIt()
        {
            _looker.Dispose();
        }
    }
}