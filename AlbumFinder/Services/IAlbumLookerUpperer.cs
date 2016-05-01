using System.Threading.Tasks;

namespace AlbumFinder.Desktop.Services
{
    internal interface IAlbumLookerUpperer
    {
        Task<string> GetJsonAsync(string artist);
    }
}