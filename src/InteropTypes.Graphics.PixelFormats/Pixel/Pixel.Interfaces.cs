using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        

        interface IDelegateProvider<TDelegate>
        {
            TDelegate GetDelegate();
        }

    }
}
