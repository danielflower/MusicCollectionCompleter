using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AlbumFinder.Desktop.Services
{
    internal class AlbumLookerUpperer : IAlbumLookerUpperer, IDisposable
    {
        private const string ITunesSearchUrl =
            "https://itunes.apple.com/search?term={artist-name}&media=music&entity=album&attribute=artistTerm";

        private readonly HttpClient httpClient = new HttpClient();

        public AlbumLookerUpperer()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        }

        public Task<string> GetJsonAsync(string artist)
        {
            string url = ITunesSearchUrl.Replace("{artist-name}", WebUtility.UrlEncode(artist));
            return httpClient.GetStringAsync(url);
        }

        public void Dispose()
        {
            httpClient.CancelPendingRequests();
            httpClient.Dispose();
        }
    }
}
