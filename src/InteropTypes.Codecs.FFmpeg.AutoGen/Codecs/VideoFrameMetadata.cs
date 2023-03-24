using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Codecs
{    
    public readonly struct VideoFrameMetadata
    {
        internal VideoFrameMetadata(IReadOnlyDictionary<string, string> a, IReadOnlyDictionary<string, long> b, IReadOnlyDictionary<string, FFmpeg.AutoGen.AVRational> c)
        {
            Info = a;
            State = b;
            Times = c;
        }

        public readonly IReadOnlyDictionary<string, string> Info;
        public readonly IReadOnlyDictionary<string, long> State;
        public readonly IReadOnlyDictionary<string, FFmpeg.AutoGen.AVRational> Times;
    }
}
