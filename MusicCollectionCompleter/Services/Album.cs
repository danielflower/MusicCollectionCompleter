using System;
using System.Text.RegularExpressions;

namespace MusicCollectionCompleter.Desktop.Services
{
    public class Album : IEquatable<Album>
    {
        public string Name { get; }
        public int Year { get; }
        public long? ItunesCollectionId { get; }
        public string NormalisedName { get; }
        public string ItunesAlbumLink { get;  }
        public string AlbumCoverUrl { get; }

        public Album(string name) : this (name, 1900, null, null, null)
        {
        }
        public Album(string name, int year, long? tunesCollectionId, string itunesAlbumLink, string albumCoverUrl)
        {
            Name = name;
            Year = year;
            ItunesCollectionId = tunesCollectionId;
            NormalisedName = Normalise(name);
            ItunesAlbumLink = itunesAlbumLink;
            AlbumCoverUrl = albumCoverUrl;
        }

        public bool Equals(Album other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(NormalisedName, other.NormalisedName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Album) obj);
        }

        public override int GetHashCode()
        {
            return NormalisedName.GetHashCode();
        }

        public override string ToString()
        {
            return (ItunesCollectionId == null) ? Name : Name + " (" + Year + ") - " + ItunesCollectionId;
        }

        public static string Normalise(string originalName)
        {
            
            string name = originalName.ToLowerInvariant();
            name = DisplayName(name);
            if (name.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring("the ".Length);
            name = name.Replace("&", "and");
            return name.Length == 0 ? originalName.Trim() : name;
        }

        public static string DisplayName(string name)
        {
            Regex regex = new Regex("[([].*[)\\]]");
            name = regex.Replace(name, "").Trim().Replace("  ", " ");
            return name;
        }
    }
    
}