namespace Metronome.Services
{
    public class Copyright
    {
        public string Source { get; }
        public string License { get; }
        public string URL { get; }
        public string Author { get; }

        public Copyright(string source,
            string author,
            string license,
            string url
            )
        {
            Source = source;
            License = license;
            URL = url;
            Author = author;
        }
    }
}
