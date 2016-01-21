namespace Metronome.Pages
{
    static class PagesHelper
    {
        public static string GetAboutPageUri()
        {
            return GetPageUri("AboutPage.xaml");
        }

        public static string GetAudioDevicePageUri()
        {
            return GetPageUri("AudioDevicePage.xaml");
        }

        public static string GetPageUri(string pageName)
        {
            return $"pack://application:,,,../Pages/{pageName}";
        }

        public static string GetAudioFilesPageUri()
        {
            return GetPageUri("AudioFilesPage.xaml");
        }
    }
}
