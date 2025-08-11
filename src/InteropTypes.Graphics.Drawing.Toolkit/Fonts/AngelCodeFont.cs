using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;

using FONTIMAGEEVALUATOR = System.Func<string, object>;


namespace InteropTypes.Graphics.Drawing.Fonts
{

    public class AngelCodeFont : IFont
    {
        #region lifecycle

        public static AngelCodeFont Load(string filePath, bool verticalFlip = false)
        {
            var finfo = new System.IO.FileInfo(filePath);
            return Load(finfo, verticalFlip);
        }

        public static AngelCodeFont Load(System.IO.FileInfo finfo, bool verticalFlip = false)
        {
            var lines = System.IO.File.ReadAllLines(finfo.FullName);

            var dir = finfo.Directory.FullName;

            return Parse(lines, fn => new System.IO.FileInfo(System.IO.Path.Combine(dir,fn)), verticalFlip);
        }

        private static AngelCodeFont Parse(string[] lines, FONTIMAGEEVALUATOR imageEvaluator, bool verticalFlip = false)
        {
            var font = AngelCodeFontDOM.BMFontTextLinesParser.Parse(lines);            

            return new AngelCodeFont(font, imageEvaluator, verticalFlip);
        }

        private AngelCodeFont(AngelCodeFontDOM.BMFont font, FONTIMAGEEVALUATOR imageEvaluator, bool verticalFlip = false)
        {            
            _Font = font;

            // prevent reloading the texture image on every letter.
            object _cachedEvaluator(string imgFileName)
            {
                if (_PagesCache.TryGetValue(imgFileName, out var cachedImg)) return cachedImg;
                cachedImg = imageEvaluator(imgFileName);
                _PagesCache.Add(imgFileName, cachedImg);
                return cachedImg;
            }

            foreach(var glyph in _Font.Chars)
            {
                _Characters[(Char)glyph.Id] = new _DeviceCharacter(glyph, font, _cachedEvaluator, verticalFlip);
            }
        }

        #endregion

        #region data
        
        private readonly AngelCodeFontDOM.BMFont _Font;

        private readonly Dictionary<string, object> _PagesCache = new Dictionary<string, object>();

        private readonly Dictionary<Char, _DeviceCharacter> _Characters = new Dictionary<char, _DeviceCharacter>();

        #endregion

        #region properties

        public bool IsVectorial => false;

        public int Height => _Font.Common.LineHeight;        

        #endregion

        #region API
        public IEnumerable<RectangleF> MeasureTextGlyphs(string text)
        {
            XY cur = new XY(0,0);            

            char prevc = default;

            foreach (var c in text)
            {
                if (!_Characters.TryGetValue(c, out var cc)) continue;

                if (c > 32)
                {
                    var gr = cc.GetGlyphRect(cur);
                    if (!gr.IsEmpty) yield return gr;
                }

                cur.X += cc.GetAdvanceX(prevc);
                prevc = c;
            }
        }

        public RectangleF MeasureTextLine(string text)
        {
            var rect = RectangleF.Empty;            

            foreach (var gr in MeasureTextGlyphs(text))
            {
                if (gr.IsEmpty) continue;
                if (rect.IsEmpty) rect = gr;
                else rect = RectangleF.Union(rect, gr);
            }            

            return rect;
        }

        public void DrawTextLineTo(ICoreCanvas2D target, Matrix3x2 transform, string text, ColorStyle tintColor)
        {
            #if DEBUG
            var previewXform = transform;
            #endif

            var delta = new XY(0, 0);            
            transform.Translation += XY.TransformNormal(delta, transform);

            char prevc = default;

            foreach (var c in text)
            {
                if (!_Characters.TryGetValue(c, out var cc)) continue;

                cc.Draw(target, transform, tintColor);

                var adv = cc.GetAdvanceX(prevc);
                prevc = c;

                delta = new XY(adv, 0);
                transform.Translation += XY.TransformNormal(delta, transform);
            }

            #if DEBUG
            _PreviewRect(target, previewXform, text);
            #endif
        }

        [Conditional("DEBUG")]
        private void _PreviewRect(ICoreCanvas2D target, Matrix3x2 transform, string text, bool singleGlyph = true)
        {
            var origin = transform.Translation;
            
            _DrawRect(target, transform, MeasureTextLine(text), COLOR.Red);

            if (singleGlyph)
            {
                foreach (var gr in MeasureTextGlyphs(text))
                {
                    _DrawRect(target, transform, gr, COLOR.Green);
                }
            }

            target.DrawEllipse(origin, 3, 3, COLOR.Blue);
        }

        private static void _DrawRect(ICoreCanvas2D target, Matrix3x2 transform, RectangleF rect, COLOR color)
        {
            var rectPoints = new Point2[5];
            InteropTypes.Graphics.Drawing.Point2.FromRect(rectPoints, rect, true);
            Point2.Transform(rectPoints, transform);
            target.DrawLines(rectPoints, 2f, color.WithOpacity(0.25f));
        }

        #endregion

        #region nested types

        class _DeviceCharacter
        {
            #region lifecycle
            public _DeviceCharacter(AngelCodeFontDOM.BMCharacter glyph, AngelCodeFontDOM.BMFont font, FONTIMAGEEVALUATOR imageEvaluator, bool verticalFlip = false)
            {
                _Glyph = glyph;

                var page = font.Pages.FirstOrDefault(p => p.Id == glyph.Page);
                var imageRef = imageEvaluator.Invoke(page.File);

                _Img = null;

                if (glyph.Id > 32 && glyph.Width > 0 && glyph.Height > 0)
                {
                    _Img = ImageSource.Create(imageRef, (glyph.X, glyph.Y), (glyph.Width, glyph.Height), (-glyph.XOffset, -glyph.YOffset + font.Common.Base), false, false, verticalFlip);

                    if (!_Img.IsVisible) _Img = null;
                }

                _OutputRect = RectangleF.Empty;
                
                if (_Img != null)
                {
                    var ppp = new XY[4];

                    var imgStyle = new ImageStyle(_Img, ColorStyle.White);
                    imgStyle.FillVertices(ppp, Matrix3x2.Identity);

                    foreach(var p in ppp)
                    {
                        _OutputRect = RectangleF.Union(_OutputRect, new RectangleF(p.X, p.Y, 0, 0));
                    }
                }

                if (font.Kernings != null)
                {
                    _Kerning = font.Kernings.Where(item => item.Second == glyph.Id).ToDictionary(kern => (char)kern.First, kern => kern.Amount);
                }

            }

            #endregion

            #region data

            private readonly AngelCodeFontDOM.BMCharacter _Glyph;
            private readonly ImageSource _Img;
            private readonly Dictionary<char, int> _Kerning;

            private readonly RectangleF _OutputRect;

            #endregion

            #region Properties
            public XY Offset => new XY(_Glyph.XOffset, _Glyph.YOffset);
            public XY Size => new XY(_Glyph.Width, _Glyph.Height);

            public int Width => _Glyph.Width;
            public int Height => _Glyph.Height;

            #endregion

            #region API

            public RectangleF GetGlyphRect(XY origin)
            {
                if (_OutputRect.IsEmpty) return RectangleF.Empty;

                var r = _OutputRect;
                r.X += origin.X;
                r.Y += origin.Y;

                return r;
            }

            public int GetAdvanceX(char prevChar)
            {
                var x = _Glyph.XAdvance;
                if (_Kerning == null || prevChar == 0) return x;

                if (_Kerning.TryGetValue(prevChar, out var kerning))
                {
                    x -= kerning;
                }

                return x;                
            }

            public void Draw(ICoreCanvas2D target, Matrix3x2 transform, ColorStyle tintColor)
            {
                if (_Img == null) return;
                target.DrawImage(transform, new ImageStyle(_Img, tintColor));
            }

            #endregion
        }        

        #endregion
    }

    internal static class AngelCodeFontDOM
    {
        // https://www.angelcode.com/products/bmfont/
        // https://www.angelcode.com/products/bmfont/doc/file_format.html

        /// <summary>
        /// Parser for fonts as found in: https://github.com/tommyettinger/fonts
        /// </summary>
        public static class BMFontTextLinesParser
        {
            public static BMFont Parse(string[] lines)
            {
                var font = new BMFont();

                foreach (var line in lines)
                {
                    if (line.StartsWith("info "))
                    {
                        font.Info = ParseInfo(line);
                    }
                    else if (line.StartsWith("common "))
                    {
                        font.Common = ParseCommon(line);
                    }
                    else if (line.StartsWith("page "))
                    {
                        font.Pages.Add(_ParsePage(line));
                    }
                    else if (line.StartsWith("char "))
                    {
                        font.Chars.Add(_ParseChar(line));
                    }
                    else if (line.StartsWith("kerning "))
                    {
                        font.Kernings.Add(_ParseKerning(line));
                    }
                }
                return font;
            }

            private static BMInfo ParseInfo(string line)
            {
                var info = new BMInfo();
                var dict = _ParseKeyValuePairs(line);
                info.Face = dict.ContainsKey("face") ? dict["face"].Trim('"') : null;
                info.Size = _GetInt(dict, "size", 0);
                // Parse other fields as needed
                return info;
            }

            private static BMCommon ParseCommon(string line)
            {
                var common = new BMCommon();
                var dict = _ParseKeyValuePairs(line);
                common.LineHeight = _GetInt(dict, "lineHeight", 0);
                common.Base = _GetInt(dict, "base", 0);
                common.ScaleW = _GetInt(dict, "scaleW", 0);
                common.ScaleH = _GetInt(dict, "scaleH", 0);
                // Parse other fields as needed
                return common;
            }

            private static BMPage _ParsePage(string line)
            {
                var page = new BMPage();
                var dict = _ParseKeyValuePairs(line);
                page.Id = _GetInt(dict, "id", 0);
                page.File = dict.ContainsKey("file") ? dict["file"].Trim('"') : null;
                return page;
            }

            private static BMCharacter _ParseChar(string line)
            {
                var ch = new BMCharacter();
                var dict = _ParseKeyValuePairs(line);
                ch.Id = _GetInt(dict, "id", 0);
                ch.X = _GetInt(dict, "x", 0);
                ch.Y = _GetInt(dict, "y", 0);
                ch.Width = _GetInt(dict, "width", 0);
                ch.Height = _GetInt(dict, "height", 0);
                ch.XOffset = _GetInt(dict, "xoffset", 0);
                ch.YOffset = _GetInt(dict, "yoffset", 0);
                ch.XAdvance = _GetInt(dict, "xadvance", 0);                
                ch.Page = _GetInt(dict, "page", 0);
                ch.Channel = _GetInt(dict, "chnl", 0);
                return ch;
            }

            private static BMKerning _ParseKerning(string line)
            {
                var kerning = new BMKerning();
                var dict = _ParseKeyValuePairs(line);
                kerning.First = _GetInt(dict, "first",0);
                kerning.Second = _GetInt(dict, "second", 0);
                kerning.Amount = _GetInt(dict, "amount", 0);
                return kerning;
            }

            private static int _GetInt(Dictionary<string,string> dict, string key, int defval)
            {
                if (!dict.TryGetValue(key, out var val)) return defval;
                if (int.TryParse(val, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out var result)) return result;
                return defval;
            }

            private static Dictionary<string, string> _ParseKeyValuePairs(string line)
            {
                var dict = new Dictionary<string, string>();                

                foreach (var part in line.Split(null))
                {
                    var idx = part.IndexOf('=');
                    if (idx < 0) continue;
                    var key = part.Substring(0, idx);
                    if (string.IsNullOrEmpty(key)) continue;
                    var val = part.Substring(idx + 1);
                    if (string.IsNullOrEmpty(val)) continue;

                    dict[key] = val;
                }
                return dict;
            }
        }

        public class BMFont
        {
            public BMInfo Info { get; set; }
            public BMCommon Common { get; set; }
            public List<BMPage> Pages { get; set; } = new List<BMPage>();
            public List<BMCharacter> Chars { get; set; } = new List<BMCharacter>();
            public List<BMKerning> Kernings { get; set; } = new List<BMKerning>();
        }

        public class BMInfo
        {
            /// <summary>
            /// This is the name of the true type font.
            /// </summary>
            public string Face { get; set; }

            /// <summary>
            /// The size of the true type font.
            /// </summary>
            public int Size { get; set; }
            
        }

        public class BMCommon
        {
            /// <summary>
            /// This is the distance in pixels between each line of text.
            /// </summary>
            public int LineHeight { get; set; }

            /// <summary>
            /// The number of pixels from the absolute top of the line to the base of the characters.
            /// </summary>
            public int Base { get; set; }

            /// <summary>
            /// The width of the texture, normally used to scale the x pos of the character image.
            /// </summary>
            public int ScaleW { get; set; }

            /// <summary>
            /// The height of the texture, normally used to scale the y pos of the character image.
            /// </summary>
            public int ScaleH { get; set; }            
        }

        public class BMPage
        {
            /// <summary>
            /// The page id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// The texture file name.
            /// </summary>
            public string File { get; set; }
        }

        public class BMCharacter
        {
            public int Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int XOffset { get; set; }
            public int YOffset { get; set; }

            /// <summary>
            /// How much the current position should be advanced after drawing the character.
            /// </summary>
            public int XAdvance { get; set; }

            /// <summary>
            /// The texture page where the character image is found.
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// The texture channel where the character image is found (1 = blue, 2 = green, 4 = red, 8 = alpha, 15 = all channels).
            /// </summary>
            public int Channel { get; set; }
        }

        public class BMKerning
        {
            /// <summary>
            /// The first character id.
            /// </summary>
            public int First { get; set; }

            /// <summary>
            /// The second character id.
            /// </summary>
            public int Second { get; set; }

            /// <summary>
            /// How much the x position should be adjusted when drawing the second character immediately following the first.
            /// </summary>
            public int Amount { get; set; }
        }
    }    
}
