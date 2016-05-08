using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace MusicCollectionCompleter.Desktop.Services
{
    internal class AlbumIgonoranceDb
    {
        private readonly DirectoryInfo _dataDir;

        public AlbumIgonoranceDb(DirectoryInfo dataDir)
        {
            _dataDir = dataDir;
            if (!_dataDir.Exists)
                _dataDir.Create();
        }

        public void IgnoreAlbum(string normalisedArtistName, Album album)
        {
            // that's right... I'm locking an interned string
            lock (string.Intern(normalisedArtistName))
            {
                File.AppendAllLines(ArtistPath(normalisedArtistName), new []{album.NormalisedName}, Encoding.UTF8);
            }
        }

        private string ArtistPath(string normalisedArtistName)
        {
            var reallyNormalisedArtistName = string.Join("_", normalisedArtistName.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(_dataDir.FullName, reallyNormalisedArtistName + ".ignored");
        }

        public IList<Album> GetIgnoredAlbums(string normalisedArtistName)
        {
            var artistPath = ArtistPath(normalisedArtistName);
            if (!File.Exists(artistPath))
                return new List<Album>();

            lock (string.Intern(normalisedArtistName))
            {
                return (from albumName in File.ReadAllLines(artistPath, Encoding.UTF8)
                    select new Album(albumName))
                    .ToList();
            }
        }
    }
}
