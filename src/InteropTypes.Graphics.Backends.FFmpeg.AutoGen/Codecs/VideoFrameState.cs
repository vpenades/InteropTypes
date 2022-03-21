using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Codecs
{
    public readonly struct VideoFrameState
    {
        internal VideoFrameState(IReadOnlyDictionary<string, string> a, IReadOnlyDictionary<string, long> b)
        {
            Info = a;
            State = b;
        }

        public readonly IReadOnlyDictionary<string, string> Info;
        public readonly IReadOnlyDictionary<string, long> State;
    }
}
