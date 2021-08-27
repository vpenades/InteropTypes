using System;
using System.Collections.Generic;
using System.Linq;

namespace InteropWith
{    
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-VR")) ProgramVR.Run(args);
            else ProgramWin.Run(args);
        }
    }
}

