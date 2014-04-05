using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Syntezator_Krawczyka
{
    public class SkładoweHarmoniczne : FalaNiestandardowa
    {
        public List<float> Składowe = new List<float>();
        public string nazwa{get;set;}
        public XmlNode xml
        {
            get
            {
                var ret = Statyczne.otwartyplik.xml.CreateElement("wave");
                var atrTyp = Statyczne.otwartyplik.xml.CreateAttribute("type");
                atrTyp.Value = "skladoweharmoniczne";
                ret.Attributes.Append(atrTyp);
                if (nazwa != null)
                {
                    var atrNazwa = Statyczne.otwartyplik.xml.CreateAttribute("name");
                    atrNazwa.Value = nazwa;
                    ret.Attributes.Append(atrNazwa);
                }
                for (var i = 0; i < Składowe.Count; i++)
                {
                    var skl = Statyczne.otwartyplik.xml.CreateElement("skladowa");
                    var atrNr = Statyczne.otwartyplik.xml.CreateAttribute("number");
                    atrNr.Value = i.ToString();
                    skl.Attributes.Append(atrNr);
                    var atrValue = Statyczne.otwartyplik.xml.CreateAttribute("value");
                    atrValue.Value = Składowe[i].ToString(CultureInfo.InvariantCulture);
                    skl.Attributes.Append(atrValue);
                    ret.AppendChild(skl);
                }
                    return ret;

            }
        }
        public SkładoweHarmoniczne()
        {
            Składowe.Add(1);
        }
        public float[] generujJedenPrzebieg(long długość)
        {
            var ret = new float[długość];
            for (int i = 0; i < Składowe.Count; i++)
            {
                var stała = Math.PI * 2 / długość * (i + 1);
                var głośność = Składowe[i];
                for (int i2 = 0; i2 < długość; i2++)
                {
                    ret[i2] += (float)Math.Sin(i2 * stała) * głośność;
                }
            }
            return ret;
        }
    }
}
