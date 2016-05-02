using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MusicCollectionCompleter.Desktop.Services
{
    class SongFileFinder
    {
        private static readonly string[] Extensions = {".mp3", ".m4a"};
        private readonly DirectoryInfo _dir;

        public SongFileFinder(DirectoryInfo dir)
        {
            if (!dir.Exists)
                throw new MusicCollectionCompleterException("Directory does not exist: " + dir.FullName);
            _dir = dir;
        }

        
        public IDisposable FindSongsAsync(IObserver<Song> songObserver)
        {
            new TaskFactory().StartNew(() =>
            {
                var unsubscribe = _dir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                    .OfType<FileInfo>()
                    .Where(f => Extensions.Contains(f.Extension.ToLowerInvariant()))
                    .Select(f => new Song(f))
                    .Subscribe(songObserver);
                return unsubscribe;
            });
            return null;
        }
    }
}