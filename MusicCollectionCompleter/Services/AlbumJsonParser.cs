using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Json;

namespace MusicCollectionCompleter.Desktop.Services
{
    internal class AlbumJsonParser
    {
        private readonly int _minimumTracks;

        public AlbumJsonParser(int minimumTracks)
        {
            _minimumTracks = minimumTracks;
        }

        public IList<Album> ToAlbums(string expectedNormalisedArtistName, string json)
        {
            var albums = new List<Album>();

            dynamic all = JsonParser.Deserialize(json.Trim());
            Dictionary<string, int> albumToTrackCount = new Dictionary<string, int>();

            foreach (Dictionary<string, object> album in all.results)
            {
                string type = (string) album["collectionType"];
                if (string.Equals("album", type, StringComparison.InvariantCultureIgnoreCase))
                {
                    var artistName = Artist.Normalise((string) album["artistName"]);
                    if (string.Equals(expectedNormalisedArtistName, artistName,
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        var trackCount = Convert.ToInt32((double) album["trackCount"]);
                        if (trackCount >= _minimumTracks)
                        {
                            string albumName = Album.DisplayName((string) album["collectionName"]);
                            var normalisedAlbumName = Album.Normalise(albumName);

                            var existing = albums.Find(other => other.NormalisedName.Equals(normalisedAlbumName));

                            var year = GetYear((string) album["releaseDate"]);
                            var tunesCollectionId = Convert.ToInt64((double) album["collectionId"]);
                            var albumUrl = (string) album["collectionViewUrl"];
                            var artUrl = (string) album["artworkUrl60"];
                            var currentAlbum = new Album(albumName, year, tunesCollectionId, albumUrl, artUrl);
                            if (existing == null)
                            {
                                albums.Add(currentAlbum);
                                albumToTrackCount[normalisedAlbumName] = trackCount;
                            }
                            else
                            {
                                bool isShorter = trackCount < albumToTrackCount[normalisedAlbumName];
                                if (isShorter)
                                {
                                    albums.Remove(existing);
                                    albums.Add(currentAlbum);
                                    albumToTrackCount[normalisedAlbumName] = trackCount;
                                }
                            }
                        }
                    }
                }
            }
            return (from a in albums
                orderby a.Year ascending, a.Name ascending
                select a).ToList();
        }

        private int GetYear(string releaseDate)
        {
            DateTime d;
            if (DateTime.TryParseExact(
                releaseDate,
                @"yyyy-MM-dd\THH:mm:ss\Z",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out d))
            {
                return d.Year;
            }
            else
            {
                return 1900;
            }
        }
    }
}