using System;

namespace MusicCollectionCompleter.Desktop.Services
{
    internal class MusicCollectionCompleterException : Exception
    {
        public MusicCollectionCompleterException(string message)
            : base(message)
        {
        }
    }
}