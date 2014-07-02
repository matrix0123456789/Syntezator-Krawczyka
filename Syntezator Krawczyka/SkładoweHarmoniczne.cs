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

        List<int> gpgpu
        {
            get
            {
                var dane = new List<int>();
                for (int i = 0; i < Składowe.Count; i++)
                {
                    dane.Add(Składowe[i].GetHashCode());
                }
                return dane;
            }
        }
        public List<float> Składowe = new List<float>();
        public string nazwa { get; set; }
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
                } zapisanePojedyńczePrzebiegi = new Dictionary<short, float[]>();
                return ret;

            }
        }
        public SkładoweHarmoniczne()
        {
            Składowe.Add(1);
            zapisanePojedyńczePrzebiegi = new Dictionary<short, float[]>();
        }
        public SkładoweHarmoniczne(XmlNode xml)
        {

            nazwa = xml.Attributes.GetNamedItem("name").Value;
            Dictionary<int, float> słownik = new Dictionary<int, float>();
            for (var i = 0; i < xml.ChildNodes.Count; i++)
            {
                słownik.Add(int.Parse(xml.ChildNodes[i].Attributes["number"].Value), float.Parse(xml.ChildNodes[i].Attributes["value"].Value, CultureInfo.InvariantCulture));
            }


            var max = słownik.Keys.Max();
            for (var i = 0; i <= max; i++)
            {
                Składowe.Add(słownik[i]);
            }
        }
        public void czyść()
        {
            zapisanePojedyńczePrzebiegi = new Dictionary<short, float[]>();
        }
        Dictionary<short, float[]> zapisanePojedyńczePrzebiegi = new Dictionary<short, float[]>();
        public float[] generujJedenPrzebieg(long długość)
        {

            if (zapisanePojedyńczePrzebiegi.ContainsKey((short)długość))
                return zapisanePojedyńczePrzebiegi[(short)długość];
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
            try { zapisanePojedyńczePrzebiegi.Add((short)długość, ret); }
            catch { }
            return ret;
        }

    }
}
