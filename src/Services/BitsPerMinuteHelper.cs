using System;
using System.Collections.Generic;

namespace Metronome.Services
{
    static class BitsPerMinuteHelper
    {
        public static string GetTempoMarking(int tempo)
        {
            var result = new List<string>();
            
            // slow.
            if (tempo <= 40)
                result.Add("grave (slow and solemn)");
            if (tempo >= 40 && tempo <= 60)
                result.Add("largo (very slow)");
            if (tempo >= 60 && tempo <= 66)
                result.Add("lento (very slow)");
            if (tempo >= 66 && tempo <= 76)
                result.Add("adagio (slow and stately)");

            // moderately.
            if (tempo >= 76 && tempo <= 108)
                result.Add("andante (at a walking pace)");
            if (tempo >= 108 && tempo <= 120)
                result.Add("moderato (moderately)");
            if (tempo >= 120 && tempo <= 132)
                result.Add("allegretto (moderately fast)");

            // fast
            if (tempo >= 120 && tempo <= 168)
                result.Add("allegro (fast and bright)");
            if (tempo >= 135 && tempo <= 145)
                result.Add("vivace (lively and fast)");
            if (tempo >= 168 && tempo <= 205)
                result.Add("presto (very fast)");
            if (tempo >= 205)
                result.Add("prestissimo (extremely fast)");

            if (result.Count == 1)
            {
                result.Add(string.Empty);
                result.Add(string.Empty);
            }
            if (result.Count == 2)
            {
                result.Add(string.Empty);
            }

            return string.Join(Environment.NewLine, result.ToArray());
        }
    }
}
