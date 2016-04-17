using System;

namespace AlbumFinder.Desktop.Services
{
    internal class AlbumFinderException : Exception
    {
        public AlbumFinderException(string message)
            : base(message)
        {
        }
    }
}