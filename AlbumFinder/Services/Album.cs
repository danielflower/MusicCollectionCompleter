using System;
using System.Text.RegularExpressions;

namespace AlbumFinder.Desktop.Services
{
    public class Album : IEquatable<Album>
    {
        public string Name { get; }
        public int Year { get; }
        public long ITunesCollectionId { get; }

        public Album(string name, int year, long tunesCollectionId)
        {
            Name = name;
            Year = year;
            ITunesCollectionId = tunesCollectionId;
        }

        public bool Equals(Album other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
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
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name + " (" + Year + ") - " + ITunesCollectionId;
        }

        public string ITunesUrl => "https://itunes.apple.com/us/album/id" + ITunesCollectionId + "?at=1001l55M";
    }
    
}