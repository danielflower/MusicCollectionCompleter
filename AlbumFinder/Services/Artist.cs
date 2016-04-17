namespace AlbumFinder.Desktop.Services
{
    public class Artist
    {
        public Artist(string name, string normalisedName)
        {
            Name = name;
            NormalisedName = normalisedName;
        }

        public string Name { get; }
        public string NormalisedName { get; }
    }
}