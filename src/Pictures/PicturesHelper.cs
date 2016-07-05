namespace Metronome.Pictures
{
    static class PicturesHelper
    {
        public static string GetImageUri(string pictureName)
        {
            return $"pack://application:,,,../Pictures/{pictureName}";
        }
    }
}
