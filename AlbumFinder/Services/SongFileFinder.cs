using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlbumFinder.Desktop.Services
{
    class SongFileFinder
    {
        private static readonly string[] Extensions = {".mp3", ".m4a"};
        private readonly DirectoryInfo _dir;


        public SongFileFinder(DirectoryInfo dir)
        {
            if (!dir.Exists)
            {
                throw new AlbumFinderException("Directory does not exist: " + dir.FullName);
            }
            _dir = dir;
        }

        
        public Task<List<Song>> FindSongsAsync(CancellationToken ct)
        {
            return Task.Factory.StartNew(() =>
            {
                return _dir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                    .OfType<FileInfo>()
                    .Where(f => Extensions.Contains(f.Extension.ToLowerInvariant()))
                    .Select(f => new Song(f))
                    .ToList();
            }, ct);
        }
        
    }
}
