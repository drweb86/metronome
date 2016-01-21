namespace Metronome.Pages
{
    static class PagesHelper
    {
        public static string GetAboutPageUri()
        {
            return GetPageUri("AboutPage.xaml");
        }

        public static string GetPageUri(string pageName)
        {
            return $"pack://application:,,,../Pages/{pageName}";
        }
    }
}
