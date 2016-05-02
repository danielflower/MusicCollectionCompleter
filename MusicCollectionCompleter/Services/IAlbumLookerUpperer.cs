using System.Threading.Tasks;

namespace MusicCollectionCompleter.Desktop.Services
{
    internal interface IAlbumLookerUpperer
    {
        Task<string> GetJsonAsync(string artist);
    }
}