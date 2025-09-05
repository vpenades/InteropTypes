using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    class Ligatures
    {

        public static IEnumerable<string> GetArabPairs(Char ligature)
        {
            switch (ligature)
            {
                // Lam + Alef with Hamza Above
                case '\uFEF5': yield return "\u0644\u0625"; break; // Isolated
                case '\uFEF6': yield return "\u0644\u0625"; break; // Final

                // Lam + Alef with Hamza Below
                case '\uFEF7': yield return "\u0644\u0622"; break; // Isolated
                case '\uFEF8': yield return "\u0644\u0622"; break; // Final

                // Lam + Alef with Madda Above
                case '\uFEF9': yield return "\u0644\u0623"; break; // Isolated
                case '\uFEFA': yield return "\u0644\u0623"; break; // Final

                // Lam + Alef
                case '\uFEFB': yield return "\u0644\u0627"; break; // Isolated
                case '\uFEFC': yield return "\u0644\u0627"; break; // Final

                // Add additional ligatures below as needed
                // Example: Arabic Ligature Allah (U+FDF2)
                case '\uFDF2': yield return "\u0627\u0644\u0644\u0647"; break;

                // Example: Arabic Ligature Akbar (U+FDF3)
                case '\uFDF3': yield return "\u0627\u0643\u0628\u0631"; break;

                // Example: Arabic Ligature Salam (U+FDF4)
                case '\uFDF4': yield return "\u0635\u0644\u0649"; break;

                // Example: Arabic Ligature Rasoul (U+FDF6)
                case '\uFDF6': yield return "\u0631\u0633\u0648\u0644"; break;
            }
        }
    }
}
