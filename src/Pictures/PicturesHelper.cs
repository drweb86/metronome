using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Metronome.Pictures
{
    static class PicturesHelper
    {
        public static string GetStop()
        {
            return GetImageUri("1452994197_player_stop.png");
        }

        public static string GetStart()
        {
            return GetImageUri("1452993892_player_play.png");
        }

        public static string GetImageUri(string pictureName)
        {
            return $"pack://application:,,,../Pictures/{pictureName}";
        }
    }
}
