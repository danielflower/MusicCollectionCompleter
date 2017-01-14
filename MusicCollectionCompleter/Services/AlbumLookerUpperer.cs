using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MusicCollectionCompleter.Desktop.Services
{
    internal class AlbumLookerUpperer : IAlbumLookerUpperer, IDisposable
    {
        private const string ItunesAffiliateId = "1001l55M";
        private const string ItunesSearchUrl =
            "https://itunes.apple.com/search?term={artist-name}&media=music&entity=album&attribute=artistTerm&at=" + ItunesAffiliateId;

        private readonly HttpClient _httpClient;
        private readonly AlbumJsonParser _parser = new AlbumJsonParser(5);
        private readonly TaskFactory _taskFactory = new TaskFactory();
        private readonly Semaphore _semaphore = new Semaphore(1, 1);

        public AlbumLookerUpperer()
        {
            ServicePointManager.DefaultConnectionLimit = 1;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public void ProcessArtistAsync(Artist artist)
        {
            _taskFactory.StartNew(() =>
            {
                _semaphore.WaitOne();

                Stopwatch stopwatch = Stopwatch.StartNew();
                GetJsonAsync(artist.Name)
                    .ContinueWith(task =>
                    {
                        stopwatch.Stop();
                        string duration = stopwatch.ElapsedMilliseconds.ToString("#,###") + "ms";
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                            Console.WriteLine("Got " + artist.Name + " info in " + duration);
                            artist.AddAvailableAlbums(_parser.ToAlbums(artist.NormalisedName, task.Result));
                        }
                        else
                        {
                            Console.WriteLine("Task failed for " + artist.Name + " after " + duration + ": " + task.Status + " - " + task.Exception);
                            // iTunes rate limits the search API now - as of writing this comment, it's around 20 per minute.
                            ProcessArtistAsync(artist);
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        _semaphore.Release();
                    });
            });
        }

        public Task<string> GetJsonAsync(string artist)
        {
            
            string url = ItunesSearchUrl.Replace("{artist-name}", WebUtility.UrlEncode(artist));
            Console.WriteLine("Calling " + url);
            return _httpClient.GetStringAsync(url);
        }

        public void Dispose()
        {
            _httpClient.CancelPendingRequests();
            _httpClient.Dispose();
        }
    }
}