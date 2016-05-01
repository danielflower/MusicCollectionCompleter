using System.IO;

namespace AlbumFinder.Desktop.Services
{
    internal class Song
    {
        public FileInfo File { get; }
        public string Artist { get; set; }
        public string Album { get; set; }

        public Song(FileInfo file)
        {
            this.File = file;
        }

        public void LoadInfo()
        {
            TagLib.File tagFile = TagLib.File.Create(File.FullName);
            Artist = tagFile.Tag.FirstAlbumArtist;
            Album = tagFile.Tag.Album;
        }

        public static string Normalise(string artist)
        {
            artist = artist.ToLowerInvariant().Trim();
            if (artist.StartsWith("the "))
                artist = artist.Substring(4);
            if (artist.StartsWith("a "))
                artist = artist.Substring(2);

            return artist;
        }

        public override string ToString()
        {
            return File.FullName;
        }
    }
}
