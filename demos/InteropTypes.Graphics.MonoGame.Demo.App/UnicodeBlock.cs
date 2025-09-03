using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDemo
{
    public readonly struct UnicodeBlock
    {
        public static HashSet<UnicodeBlock> GetUnicodeBlocks(string input)
        {
            var blocks = new HashSet<UnicodeBlock>();
            foreach (var ch in input)
            {
                // Handle surrogate pairs
                if (char.IsHighSurrogate(ch)) continue;

                bool found = false;
                foreach (var block in GetUnicodeBlocks())
                {
                    if (block.Contains(ch)) { blocks.Add(block); { found = true; break; } }
                }

                if (!found) blocks.Add(new UnicodeBlock("Undefined",ch));
            }

            // Handle supplementary characters (surrogate pairs)
            for (int i = 0; i < input.Length-1; i++)
            {
                if (char.IsHighSurrogate(input[i]) && char.IsLowSurrogate(input[i + 1]))
                {
                    int codepoint = char.ConvertToUtf32(input, i);
                    foreach (var block in GetUnicodeBlocks())
                    {
                        if (block.Contains(codepoint)) { blocks.Add(block); break; }
                    }

                    i++; // Skip low surrogate
                }
            }
            return blocks;
        }


        // https://www.unicode.org/Public/UNIDATA/Blocks.txt
        private static IEnumerable<UnicodeBlock> GetUnicodeBlocks()
        {
            yield return new UnicodeBlock(0x0000, 0x007F, "Basic Latin");
            yield return new UnicodeBlock(0x0080, 0x00FF, "Latin-1 Supplement");
            yield return new UnicodeBlock(0x0100, 0x017F, "Latin Extended-A");
            yield return new UnicodeBlock(0x0180, 0x024F, "Latin Extended-B");
            yield return new UnicodeBlock(0x0250, 0x02AF, "IPA Extensions");
            yield return new UnicodeBlock(0x02B0, 0x02FF, "Spacing Modifier Letters");
            yield return new UnicodeBlock(0x0300, 0x036F, "Combining Diacritical Marks");
            yield return new UnicodeBlock(0x0370, 0x03FF, "Greek and Coptic");
            yield return new UnicodeBlock(0x0400, 0x04FF, "Cyrillic");
            yield return new UnicodeBlock(0x0500, 0x052F, "Cyrillic Supplement");
            yield return new UnicodeBlock(0x0530, 0x058F, "Armenian");
            yield return new UnicodeBlock(0x0590, 0x05FF, "Hebrew");
            yield return new UnicodeBlock(0x0600, 0x06FF, "Arabic");
            yield return new UnicodeBlock(0x0700, 0x074F, "Syriac");
            yield return new UnicodeBlock(0x0750, 0x077F, "Arabic Supplement");
            yield return new UnicodeBlock(0x0780, 0x07BF, "Thaana");
            yield return new UnicodeBlock(0x07C0, 0x07FF, "NKo");
            yield return new UnicodeBlock(0x0800, 0x083F, "Samaritan");
            yield return new UnicodeBlock(0x0840, 0x085F, "Mandaic");
            yield return new UnicodeBlock(0x08A0, 0x08FF, "Arabic Extended-A");
            yield return new UnicodeBlock(0x0900, 0x097F, "Devanagari");
            yield return new UnicodeBlock(0x0980, 0x09FF, "Bengali");
            yield return new UnicodeBlock(0x0A00, 0x0A7F, "Gurmukhi");
            yield return new UnicodeBlock(0x0A80, 0x0AFF, "Gujarati");
            yield return new UnicodeBlock(0x0B00, 0x0B7F, "Oriya");
            yield return new UnicodeBlock(0x0B80, 0x0BFF, "Tamil");
            yield return new UnicodeBlock(0x0C00, 0x0C7F, "Telugu");
            yield return new UnicodeBlock(0x0C80, 0x0CFF, "Kannada");
            yield return new UnicodeBlock(0x0D00, 0x0D7F, "Malayalam");
            yield return new UnicodeBlock(0x0D80, 0x0DFF, "Sinhala");
            yield return new UnicodeBlock(0x0E00, 0x0E7F, "Thai");
            yield return new UnicodeBlock(0x0E80, 0x0EFF, "Lao");
            yield return new UnicodeBlock(0x0F00, 0x0FFF, "Tibetan");
            yield return new UnicodeBlock(0x1000, 0x109F, "Myanmar");
            yield return new UnicodeBlock(0x10A0, 0x10FF, "Georgian");
            yield return new UnicodeBlock(0x1100, 0x11FF, "Hangul Jamo");
            yield return new UnicodeBlock(0x1200, 0x137F, "Ethiopic");
            yield return new UnicodeBlock(0x13A0, 0x13FF, "Cherokee");
            yield return new UnicodeBlock(0x1400, 0x167F, "Unified Canadian Aboriginal Syllabics");
            yield return new UnicodeBlock(0x1680, 0x169F, "Ogham");
            yield return new UnicodeBlock(0x16A0, 0x16FF, "Runic");
            yield return new UnicodeBlock(0x1700, 0x171F, "Tagalog");
            yield return new UnicodeBlock(0x1720, 0x173F, "Hanunoo");
            yield return new UnicodeBlock(0x1740, 0x175F, "Buhid");
            yield return new UnicodeBlock(0x1760, 0x177F, "Tagbanwa");
            yield return new UnicodeBlock(0x1780, 0x17FF, "Khmer");
            yield return new UnicodeBlock(0x1800, 0x18AF, "Mongolian");
            yield return new UnicodeBlock(0x18B0, 0x18FF, "Unified Canadian Aboriginal Syllabics Extended");
            yield return new UnicodeBlock(0x1900, 0x194F, "Limbu");
            yield return new UnicodeBlock(0x1950, 0x197F, "Tai Le");
            yield return new UnicodeBlock(0x1980, 0x19DF, "New Tai Lue");
            yield return new UnicodeBlock(0x19E0, 0x19FF, "Khmer Symbols");
            yield return new UnicodeBlock(0x1A00, 0x1A1F, "Buginese");
            yield return new UnicodeBlock(0x1A20, 0x1AAF, "Tai Tham");
            yield return new UnicodeBlock(0x1B00, 0x1B7F, "Balinese");
            yield return new UnicodeBlock(0x1B80, 0x1BBF, "Sundanese");
            yield return new UnicodeBlock(0x1BC0, 0x1BFF, "Batak");
            yield return new UnicodeBlock(0x1C00, 0x1C4F, "Lepcha");
            yield return new UnicodeBlock(0x1C50, 0x1C7F, "Ol Chiki");
            yield return new UnicodeBlock(0x1CC0, 0x1CCF, "Sundanese Supplement");
            yield return new UnicodeBlock(0x1CD0, 0x1CFF, "Vedic Extensions");
            yield return new UnicodeBlock(0x1D00, 0x1D7F, "Phonetic Extensions");
            yield return new UnicodeBlock(0x1D80, 0x1DBF, "Phonetic Extensions Supplement");
            yield return new UnicodeBlock(0x1DC0, 0x1DFF, "Combining Diacritical Marks Supplement");
            yield return new UnicodeBlock(0x1E00, 0x1EFF, "Latin Extended Additional");
            yield return new UnicodeBlock(0x1F00, 0x1FFF, "Greek Extended");
            yield return new UnicodeBlock(0x2000, 0x206F, "General Punctuation");
            yield return new UnicodeBlock(0x2070, 0x209F, "Superscripts and Subscripts");
            yield return new UnicodeBlock(0x20A0, 0x20CF, "Currency Symbols");
            yield return new UnicodeBlock(0x20D0, 0x20FF, "Combining Diacritical Marks for Symbols");
            yield return new UnicodeBlock(0x2100, 0x214F, "Letterlike Symbols");
            yield return new UnicodeBlock(0x2150, 0x218F, "Number Forms");
            yield return new UnicodeBlock(0x2190, 0x21FF, "Arrows");
            yield return new UnicodeBlock(0x2200, 0x22FF, "Mathematical Operators");
            yield return new UnicodeBlock(0x2300, 0x23FF, "Miscellaneous Technical");
            yield return new UnicodeBlock(0x2400, 0x243F, "Control Pictures");
            yield return new UnicodeBlock(0x2440, 0x245F, "Optical Character Recognition");
            yield return new UnicodeBlock(0x2460, 0x24FF, "Enclosed Alphanumerics");
            yield return new UnicodeBlock(0x2500, 0x257F, "Box Drawing");
            yield return new UnicodeBlock(0x2580, 0x259F, "Block Elements");
            yield return new UnicodeBlock(0x25A0, 0x25FF, "Geometric Shapes");
            yield return new UnicodeBlock(0x2600, 0x26FF, "Miscellaneous Symbols");
            yield return new UnicodeBlock(0x2700, 0x27BF, "Dingbats");
            yield return new UnicodeBlock(0x27C0, 0x27EF, "Miscellaneous Mathematical Symbols-A");
            yield return new UnicodeBlock(0x27F0, 0x27FF, "Supplemental Arrows-A");
            yield return new UnicodeBlock(0x2800, 0x28FF, "Braille Patterns");
            yield return new UnicodeBlock(0x2900, 0x297F, "Supplemental Arrows-B");
            yield return new UnicodeBlock(0x2980, 0x29FF, "Miscellaneous Mathematical Symbols-B");
            yield return new UnicodeBlock(0x2A00, 0x2AFF, "Supplemental Mathematical Operators");
            yield return new UnicodeBlock(0x2B00, 0x2BFF, "Miscellaneous Symbols and Arrows");
            yield return new UnicodeBlock(0x2C00, 0x2C5F, "Glagolitic");
            yield return new UnicodeBlock(0x2C60, 0x2C7F, "Latin Extended-C");
            yield return new UnicodeBlock(0x2C80, 0x2CFF, "Coptic");
            yield return new UnicodeBlock(0x2D00, 0x2D2F, "Georgian Supplement");
            yield return new UnicodeBlock(0x2D30, 0x2D7F, "Tifinagh");
            yield return new UnicodeBlock(0x2D80, 0x2DDF, "Ethiopic Extended");
            yield return new UnicodeBlock(0x2DE0, 0x2DFF, "Cyrillic Extended-A");
            yield return new UnicodeBlock(0x2E00, 0x2E7F, "Supplemental Punctuation");
            yield return new UnicodeBlock(0x2E80, 0x2EFF, "CJK Radicals Supplement");
            yield return new UnicodeBlock(0x2F00, 0x2FDF, "Kangxi Radicals");
            yield return new UnicodeBlock(0x2FF0, 0x2FFF, "Ideographic Description Characters");
            yield return new UnicodeBlock(0x3000, 0x303F, "CJK Symbols and Punctuation");
            yield return new UnicodeBlock(0x3040, 0x309F, "Hiragana");
            yield return new UnicodeBlock(0x30A0, 0x30FF, "Katakana");
            yield return new UnicodeBlock(0x3100, 0x312F, "Bopomofo");
            yield return new UnicodeBlock(0x3130, 0x318F, "Hangul Compatibility Jamo");
            yield return new UnicodeBlock(0x3190, 0x319F, "Kanbun");
            yield return new UnicodeBlock(0x31A0, 0x31BF, "Bopomofo Extended");
            yield return new UnicodeBlock(0x31C0, 0x31EF, "CJK Strokes");
            yield return new UnicodeBlock(0x31F0, 0x31FF, "Katakana Phonetic Extensions");
            yield return new UnicodeBlock(0x3200, 0x32FF, "Enclosed CJK Letters and Months");
            yield return new UnicodeBlock(0x3300, 0x33FF, "CJK Compatibility");
            yield return new UnicodeBlock(0x3400, 0x4DBF, "CJK Unified Ideographs Extension A");
            yield return new UnicodeBlock(0x4DC0, 0x4DFF, "Yijing Hexagram Symbols");
            yield return new UnicodeBlock(0x4E00, 0x9FFF, "CJK Unified Ideographs");
            yield return new UnicodeBlock(0xA000, 0xA48F, "Yi Syllables");
            yield return new UnicodeBlock(0xA490, 0xA4CF, "Yi Radicals");
            yield return new UnicodeBlock(0xA4D0, 0xA4FF, "Lisu");
            yield return new UnicodeBlock(0xA500, 0xA63F, "Vai");
            yield return new UnicodeBlock(0xA640, 0xA69F, "Cyrillic Extended-B");
            yield return new UnicodeBlock(0xA6A0, 0xA6FF, "Bamum");
            yield return new UnicodeBlock(0xA700, 0xA71F, "Modifier Tone Letters");
            yield return new UnicodeBlock(0xA720, 0xA7FF, "Latin Extended-D");
            yield return new UnicodeBlock(0xA800, 0xA82F, "Syloti Nagri");
            yield return new UnicodeBlock(0xA830, 0xA83F, "Common Indic Number Forms");
            yield return new UnicodeBlock(0xA840, 0xA87F, "Phags-pa");
            yield return new UnicodeBlock(0xA880, 0xA8DF, "Saurashtra");
            yield return new UnicodeBlock(0xA8E0, 0xA8FF, "Devanagari Extended");
            yield return new UnicodeBlock(0xA900, 0xA92F, "Kayah Li");
            yield return new UnicodeBlock(0xA930, 0xA95F, "Rejang");
            yield return new UnicodeBlock(0xA960, 0xA97F, "Hangul Jamo Extended-A");
            yield return new UnicodeBlock(0xA980, 0xA9DF, "Javanese");
            yield return new UnicodeBlock(0xA9E0, 0xA9FF, "Myanmar Extended-B");
            yield return new UnicodeBlock(0xAA00, 0xAA5F, "Cham");
            yield return new UnicodeBlock(0xAA60, 0xAA7F, "Myanmar Extended-A");
            yield return new UnicodeBlock(0xAA80, 0xAADF, "Tai Viet");
            yield return new UnicodeBlock(0xAAE0, 0xAAFF, "Meetei Mayek Extensions");
            yield return new UnicodeBlock(0xAB00, 0xAB2F, "Ethiopic Extended-A");
            yield return new UnicodeBlock(0xAB30, 0xAB6F, "Latin Extended-E");
            yield return new UnicodeBlock(0xAB70, 0xABBF, "Cherokee Supplement");
            yield return new UnicodeBlock(0xABC0, 0xABFF, "Meetei Mayek");
            yield return new UnicodeBlock(0xAC00, 0xD7AF, "Hangul Syllables");
            yield return new UnicodeBlock(0xD7B0, 0xD7FF, "Hangul Jamo Extended-B");
            yield return new UnicodeBlock(0xD800, 0xDB7F, "High Surrogates");
            yield return new UnicodeBlock(0xDB80, 0xDBFF, "High Private Use Surrogates");
            yield return new UnicodeBlock(0xDC00, 0xDFFF, "Low Surrogates");
            yield return new UnicodeBlock(0xE000, 0xF8FF, "Private Use Area");
            yield return new UnicodeBlock(0xF900, 0xFAFF, "CJK Compatibility Ideographs");
            yield return new UnicodeBlock(0xFB00, 0xFB4F, "Alphabetic Presentation Forms");
            yield return new UnicodeBlock(0xFB50, 0xFDFF, "Arabic Presentation Forms-A");
            yield return new UnicodeBlock(0xFE00, 0xFE0F, "Variation Selectors");
            yield return new UnicodeBlock(0xFE10, 0xFE1F, "Vertical Forms");
            yield return new UnicodeBlock(0xFE20, 0xFE2F, "Combining Half Marks");
            yield return new UnicodeBlock(0xFE30, 0xFE4F, "CJK Compatibility Forms");
            yield return new UnicodeBlock(0xFE50, 0xFE6F, "Small Form Variants");
            yield return new UnicodeBlock(0xFE70, 0xFEFF, "Arabic Presentation Forms-B");
            yield return new UnicodeBlock(0xFF00, 0xFFEF, "Halfwidth and Fullwidth Forms");
            yield return new UnicodeBlock(0xFFF0, 0xFFFF, "Specials");
            // Add more blocks as needed (e.g., Supplementary Multilingual Plane blocks)
        }

        private UnicodeBlock(int start, int end, string name)
        {
            Start = start;
            End = end;
            Name = name;
        }

        private UnicodeBlock(string name, char ch)
        {
            Start = End = (int)ch;            
            Name = name;
        }

        /// <summary>
        /// First character (inclusive)
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Last character (inclusive)
        /// </summary>
        public int End { get; }

        /// <summary>
        /// block name
        /// </summary>
        public string Name { get; }

        public bool Contains(char character)
        {
            return Contains((int)character);
        }

        public bool Contains(int code)
        {
            return code >= Start && code <= End;
        }
        
    }
}


        
