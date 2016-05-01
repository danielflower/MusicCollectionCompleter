using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Json;

namespace AlbumFinder.Desktop.Services
{
    internal class AlbumJsonParser
    {
        private readonly int _minimumTracks;

        public AlbumJsonParser(int minimumTracks)
        {
            _minimumTracks = minimumTracks;
        }

        public IList<Album> ToAlbums(string json)
        {
            var albums = new List<Album>();

            dynamic all = JsonParser.Deserialize(json.Trim());
            Dictionary<string, int> albumToTrackCount = new Dictionary<string, int>();

            foreach (Dictionary<string, object> album in all.results)
            {
                string type = (string) album["collectionType"];
                if (string.Equals("album", type, StringComparison.InvariantCultureIgnoreCase))
                {
                    var artistName = (string)album["artistName"];
                    if (!string.Equals("Various Artists", artistName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var trackCount = Convert.ToInt32((double)album["trackCount"]);
                        if (trackCount >= _minimumTracks)
                        {
                            var name = Normalise((string) album["collectionName"]);

                            var existing = albums.Find(other => other.Name.Equals(name));

                            var year = GetYear((string) album["releaseDate"]);
                            var tunesCollectionId = Convert.ToInt64((double) album["collectionId"]);
                            var currentAlbum = new Album(name, year, tunesCollectionId);
                            if (existing == null)
                            {
                                albums.Add(currentAlbum);
                                albumToTrackCount[name] = trackCount;
                            }
                            else
                            {
                                bool isShorter = trackCount < albumToTrackCount[name];
                                if (isShorter)
                                {
                                    albums.Remove(existing);
                                    albums.Add(currentAlbum);
                                    albumToTrackCount[name] = trackCount;
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

        public static string Normalise(string originalName)
        {
            Regex regex = new Regex("\\(.*\\)");
            string name = regex.Replace(originalName, "").Trim().Replace("  ", " ");
            if (name.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring("the ".Length);
            return name.Length == 0 ? originalName.Trim() : name;
        }
    }
}