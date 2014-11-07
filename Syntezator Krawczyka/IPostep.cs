using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    interface IPostep
    {
        long value { get; }
        long max { get; }
    }
}
