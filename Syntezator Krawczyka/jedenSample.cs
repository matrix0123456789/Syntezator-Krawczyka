using Syntezator_Krawczyka.Synteza;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace Syntezator_Krawczyka
{
    public class jedenSample : IodDo
    {

        public event Action zmianaDługości;
        public void zmienionoDługość()
        {
            if (zmianaDługości != null)
                zmianaDługości();
        }
        public sample sample;
        public float głośność { get; set; }
        public long delay { get; set; }
        public XmlNode xml;
        public long start = 0;
        public long end = int.MaxValue;
        public long id;
        public jedenSample()
        {
            id = nuta.nowyid;
            głośność = 1;
            granie.graniestart();
            xml = Statyczne.otwartyplik.xml.CreateElement("sample");
            Statyczne.otwartyplik.xml.DocumentElement.AppendChild(xml);

        }

        public jedenSample(string x)
            : this()
        {
            if (Statyczne.otwartyplik.wszytskieSamplePliki.ContainsKey(x))
                sample = Statyczne.otwartyplik.wszytskieSamplePliki[x];
            else
            {
                sample = new sample(x);
                Statyczne.otwartyplik.wszytskieSamplePliki.Add(x, sample);
            }
            sample.load += sample_load;
            var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("file");
            atrybut.Value = x;
            xml.Attributes.SetNamedItem(atrybut);
            var atrybut2 = Statyczne.otwartyplik.xml.CreateAttribute("orginalFile");
            atrybut2.Value = x;
            xml.Attributes.SetNamedItem(atrybut2);
        }

        void sample_load()
        {
            if (end == int.MaxValue)
                end = sample.max;
        }

        public jedenSample(XmlNode xml)
            : this()
        {

            granie.graniestart();
            this.xml = xml;

            if (xml.Attributes["delay"] != null)
            {
                delay = (long)(float.Parse((xml.Attributes["delay"].Value), CultureInfo.InvariantCulture) * plik.Hz * 60 / plik.tempo);
                delayUstawione = double.Parse((xml.Attributes["delay"].Value), CultureInfo.InvariantCulture);
            }
            if (xml.Attributes["start"] != null)
            {
                start = (long)(float.Parse((xml.Attributes["start"].Value), CultureInfo.InvariantCulture));
            }
            if (xml.Attributes["end"] != null)
            {
                end = (long)(float.Parse((xml.Attributes["end"].Value), CultureInfo.InvariantCulture));
            }

            if (Statyczne.otwartyplik.wszytskieSamplePliki.ContainsKey(xml.Attributes["file"].Value))
                sample = Statyczne.otwartyplik.wszytskieSamplePliki[xml.Attributes["file"].Value];
            else
            {
                sample = new sample(xml.Attributes["file"].Value);
                Statyczne.otwartyplik.wszytskieSamplePliki.Add(xml.Attributes["file"].Value, sample);
            }
            sample.load += sample_load;
        }
        internal void działaj()
        {

            float[,] dane;
            int dl;
            while (sample.fala == null)
                Thread.Sleep(100);
            var l = sample.fala.GetLength(1);
            var zmianaCzęstotliwości = plik.Hz / sample.częstotliwość;
            if (zmianaCzęstotliwości == 1)
            {
                dane = sample.fala;
                dl = l;
            }
            else
            {
                dl = (int)Math.Ceiling(sample.fala.GetLength(1) * zmianaCzęstotliwości);
                dane = new float[sample.fala.GetLength(0), dl];
                for (byte k = 0; k < sample.kanały; k++)//TODO do optymalizacji
                    for (var i2 = 0; dl > i2; i2++)
                    {
                        var dz = (i2) / zmianaCzęstotliwości;
                        if (dz + 1 < l)
                            dane[k, i2] = ((sample.fala[k, (int)Math.Floor(dz)] * ((i2 / zmianaCzęstotliwości) % 1)) + (sample.fala[k, (int)Math.Ceiling(dz)] * (1 - (i2 / zmianaCzęstotliwości) % 1))) * głośność;

                        else if ((int)Math.Floor(dz) + 1 < l)
                        {
                            dane[k, i2] = (sample.fala[k, (int)Math.Floor(dz)]) * głośność;
                        }
                        //debugowanie


                    }
            }


            if (granie.wynik == null)
            {
                lock (granie.grają)
                {
                    if (granie.grają.ContainsKey(id))
                    {
                        granie.grają[id].dźwiękWielokanałowy = dane;
                        granie.grają[id].nuta = null;
                        granie.grają[id].zagrano = (long)(start * plik.Hz / sample.częstotliwość);
                    }
                    else
                    {
                        var gratym = new gra(dane);
                        gratym.zagrano = (long)(start * plik.Hz / sample.częstotliwość);
                        granie.grają.Add(id, gratym);

                    }
                    granie.grajRaz();
                }
            }
            else
            {


                long i = delay;
                var opt1 = -delay + (long)(start * plik.Hz / sample.częstotliwość);

                var opt3 = (long)(end * plik.Hz / sample.częstotliwość) - opt1;
                try
                {
                    /*if (głośność == 1)

                        if (input.balans0 == 1 && input.balans1 == 1)
                            for (; i < opt3; i++)
                            {
                                wynik[0, i] += input.dane[i + opt1];
                                wynik[1, i] += input.dane[i + opt1];
                            }
                        else
                            for (; i < opt3; i++)
                            {
                                wynik[0, i] += input.dane[i + opt1] * input.balans0;
                                wynik[1, i] += input.dane[i + opt1] * input.balans1;
                            }
                    else
                    {
                        var mn0 = input.głośność * input.balans0;
                        var mn1 = input.głośność * input.balans1;
                        for (; i < opt3; i++)
                        {
                            wynik[0, i] += input.dane[i + opt1] * mn0;
                            wynik[1, i] += input.dane[i + opt1] * mn1;
                        }
                    }*/
                    if (sample.kanały == 1)
                    {
                        for (; i < opt3; i++)
                        {
                            granie.wynik[0, i] += dane[0, i + opt1] * głośność;
                            granie.wynik[1, i] += dane[0, i + opt1] * głośność;
                        }
                    }
                    else
                    {
                        for (; i < opt3; i++)
                        {
                            granie.wynik[0, i] += dane[0, i + opt1] * głośność;
                            granie.wynik[1, i] += dane[1, i + opt1] * głośność;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }


            }

        }


        public long dlugosc
        {
            get
            {
                if (sample.fala == null)
                    return 0;
                return sample.fala.GetLongLength(1);
            }
        }
        public long dlugoscGrana
        {
            get
            {
                if (sample.fala == null)
                    return 0;
                var mdl = sample.fala.GetLongLength(1);
                if (mdl < end)
                    return sample.fala.GetLongLength(1) - start;
                else
                    return end - start;
            }
        }
        public override string ToString()
        {
            return sample.plik.Substring(sample.plik.LastIndexOfAny(new char[] { '\\', '/' }) + 1);
        }

        public double delayUstawione;
    }
}
