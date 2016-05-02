using System;
using System.Text.RegularExpressions;

namespace AlbumFinder.Desktop.Services
{
    public class Album : IEquatable<Album>
    {
        public string Name { get; }
        public int Year { get; }
        public long? ITunesCollectionId { get; }
        public string NormalisedName { get; }

        public Album(string name) : this (name, 1900, null)
        {
        }
        public Album(string name, int year, long? tunesCollectionId)
        {
            Name = name;
            Year = year;
            ITunesCollectionId = tunesCollectionId;
            NormalisedName = Normalise(name);
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
            return (ITunesCollectionId == null) ? Name : Name + " (" + Year + ") - " + ITunesCollectionId;
        }

        public string ITunesUrl => (ITunesCollectionId == null) ? null : "https://itunes.apple.com/us/album/id" + ITunesCollectionId + "?at=1001l55M";

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