using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Syntezator_Krawczyka
{
    public interface FalaNiestandardowa
    {
        float[] generujJedenPrzebieg(long długość);
        XmlNode xml{get;}
        string nazwa { get; set; }
    }
}
