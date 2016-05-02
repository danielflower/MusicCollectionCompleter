using System.IO;

namespace MusicCollectionCompleter.Desktop.Services
{
    internal class Song
    {
        public FileInfo File { get; }
        public string Artist { get; set; }
        public string Album { get; set; }

        public Song(FileInfo file)
        {
            File = file;
        }

        public void LoadInfo()
        {
            TagLib.File tagFile = TagLib.File.Create(File.FullName);
            Artist = tagFile.Tag.FirstAlbumArtist;
            Album = tagFile.Tag.Album;
        }

        public override string ToString()
        {
            return File.FullName;
        }
    }
}
