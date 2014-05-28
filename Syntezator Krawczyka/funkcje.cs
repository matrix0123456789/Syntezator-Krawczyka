using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Syntezator_Krawczyka
{
    
    static class funkcje
    {

        //static byte[] puste2=null;
        static public bool graj(float[] fala, double głośność)
        {
            var bufor = new byte[fala.Length * 2];
            for (var i = 0; i < fala.Length; i++)
            {
                short liczba;
                //if(fala[i]>=0)
                liczba = (short)(fala[i] * głośność * short.MaxValue);
                //else
                //liczba = (short)(fala[i] * głośność * -short.MaxValue);
                bufor[2 * i + 1] = (byte)Math.Floor(liczba / 256f);
                bufor[2 * i] = (byte)(liczba % 256);
            }
        bufordodaj:
            try
            {
                Statyczne.bufor.AddSamples(bufor, 0, fala.Length * 2);
            }
            catch (InvalidOperationException)
            {
                Statyczne.bufor.ClearBuffer();
                goto bufordodaj;
            }

            return false;


        }
        static public bool graj(float[,] fala)
        {
            var bufor = new byte[fala.Length * 2];
            for (var i = 0; i < fala.Length/2; i++)
            {
                short liczba;
                liczba = (short)(fala[0,i] * short.MaxValue);
                bufor[4 * i + 1] = (byte)Math.Floor(liczba / 256f);
                bufor[4 * i] = (byte)(liczba % 256);
                liczba = (short)(fala[1,i] * short.MaxValue);
                bufor[4 * i + 3] = (byte)Math.Floor(liczba / 256f);
                bufor[4 * i+2] = (byte)(liczba % 256);
            }
        bufordodaj:
            try
            {
                Statyczne.bufor.AddSamples(bufor, 0, fala.Length * 2);
            }
            catch (InvalidOperationException)
            {
                Statyczne.bufor.ClearBuffer();
                goto bufordodaj;
            }

            return false;


        }
        /// <summary>
        /// Zapisuje dźwięk do pliku wave
        /// </summary>
        /// <param name="fala"></param>
        /// <param name="głośność"></param>
        /// <param name="plik">ścierzka do pliku</param>
        static public void zapisz(float[,] fala, string plik)
        {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(plik, false);
                System.IO.BinaryWriter write = new System.IO.BinaryWriter(sw.BaseStream);
                var p = wave(fala);
                write.Write(p);


        }
        /// <summary>
        /// Tworzy plik wave
        /// </summary>
        /// <param name="fala"></param>
        /// <param name="głośność"></param>
        /// <returns>Tablica odpowiadająca plikowi wave</returns>
        static public byte[] wave(float[] fala)
        {
            char[] pus = Syntezator_Krawczyka.Properties.Resources.czysty.ToCharArray();
            byte[] puste = new byte[pus.Length + fala.Length * 2 - 2];
            for (int i = 0; i < pus.Length && i < puste.Length; i++)
            {
                puste[i] = (byte)pus[i];
            }
            byte[] rozmiar = BitConverter.GetBytes(puste.Length - 8);
            puste[4] = rozmiar[0];
            puste[5] = rozmiar[1];
            puste[6] = rozmiar[2];
            puste[7] = rozmiar[3];
            byte[] rozmiar2 = BitConverter.GetBytes(fala.Length * 2);
            puste[40] = rozmiar2[0];
            puste[41] = rozmiar2[1];
            puste[42] = rozmiar2[2];
            puste[43] = rozmiar2[3];


            byte[] czestotliwosc = BitConverter.GetBytes((int)plik.Hz);
            puste[24] = czestotliwosc[0];
            puste[25] = czestotliwosc[1];
            puste[26] = czestotliwosc[2];
            puste[27] = czestotliwosc[3];
            byte[] czestotliwosc2 = BitConverter.GetBytes((int)plik.Hz * 4);
            puste[28] = czestotliwosc2[0];
            puste[29] = czestotliwosc2[1];
            puste[30] = czestotliwosc2[2];
            puste[31] = czestotliwosc2[3];
            puste[32] = 4;
            long falai = 0;
            for (int z = pus.Length - 2; z < puste.Length && fala.LongLength > falai; z = z + 2)
            {

                if (fala[falai] >= 1)
                    puste[z] = 127;
                else if (fala[falai] >= 0)
                    puste[z] = (byte)(fala[falai] * 127);
                else if (fala[falai] > -1)
                    puste[z] = (byte)((1 + fala[falai]) * 127 + 128);
                else
                    puste[z] = 128;
                puste[z - 1] = (byte)((fala[falai] * 127 * 256) % 256);
                //puste[z + 1] = 0;
                falai++;

            }
            return puste;
        }
        static public byte[] wave(float[,] fala)
        {
            char[] pus = Syntezator_Krawczyka.Properties.Resources.czysty.ToCharArray();
            byte[] puste = new byte[pus.Length + fala.Length * 2 - 2];
            for (int i = 0; i < pus.Length && i < puste.Length; i++)
            {
                puste[i] = (byte)pus[i];
            }
            byte[] rozmiar = BitConverter.GetBytes(puste.Length - 8);
            puste[4] = rozmiar[0];
            puste[5] = rozmiar[1];
            puste[6] = rozmiar[2];
            puste[7] = rozmiar[3];
            byte[] rozmiar2 = BitConverter.GetBytes(fala.Length * 2);
            puste[40] = rozmiar2[0];
            puste[41] = rozmiar2[1];
            puste[42] = rozmiar2[2];
            puste[43] = rozmiar2[3];


            byte[] czestotliwosc = BitConverter.GetBytes((int)plik.Hz);
            puste[24] = czestotliwosc[0];
            puste[25] = czestotliwosc[1];
            puste[26] = czestotliwosc[2];
            puste[27] = czestotliwosc[3];
            byte[] czestotliwosc2 = BitConverter.GetBytes((int)plik.Hz * 4);
            puste[28] = czestotliwosc2[0];
            puste[29] = czestotliwosc2[1];
            puste[30] = czestotliwosc2[2];
            puste[31] = czestotliwosc2[3];
            puste[32] = 4;



            puste[22] = 2;
            long falai = 0;
            for (int z = pus.Length + 2; z < puste.Length && fala.LongLength/2 > falai; z = z + 4)
            {

                if (fala[0, falai] > .98f)
                    fala[0, falai] = .98f;
                else if (fala[0, falai] < -.98f)
                    fala[0, falai] = -0.98f;
                if (fala[1, falai] > .98f)
                    fala[1, falai] = .98f;
                else if (fala[1, falai] < -.98f)
                    fala[1, falai] = -.98f;

                if (fala[0, falai] >= 0)
                    puste[z-2] = (byte)(fala[0, falai] * 128);
                else
                    puste[z-2] = (byte)((1 + fala[0, falai]) * 128 + 128);
                puste[z - 3] = (byte)((fala[0, falai] * 128 * 256) % 256);
                if (fala[1, falai] >= 0)
                    puste[z] = (byte)(fala[1, falai] * 128);
                else
                    puste[z] = (byte)((1 + fala[1, falai]) * 128 + 128);
                puste[z - 1] = (byte)((fala[1, falai] * 128 * 256) % 256);
                //puste[z + 1] = 0;
                falai++;

            }
            return puste;
        }
        /// <summary>
        /// Wylicza częstotliwość nuty na podstawie oktawy i tonu
        /// </summary>
        /// <param name="oktawa"></param>
        /// <param name="ton">Ilość całych tonów powyrzej C (C to 0)</param>
        /// <returns>Częstotliwość w hercach</returns>
        public static double częstotliwość(short oktawa, float ton)
        {
            return 130.812783 * Math.Pow(2, (oktawa) + (ton / 6));
        }
        public static double ilepróbek(short oktawa, float ton)
        {
            return plik.Hz/(130.812783 * Math.Pow(2, (oktawa) + (ton / 6)));
        }
        public static double ton(double ileprobek)
        {
            return Math.Log(plik.Hz/(130.812783*ileprobek),2)*6;
        }

        public static XmlNode klonujXML(XmlDocument doc, System.Xml.XmlNode wej)
        {
            if(wej.NodeType==XmlNodeType.Element)
            {
                var ret = doc.CreateElement(wej.Name);
                foreach (XmlAttribute x in wej.Attributes)
                {
                    XmlAttribute nowyAtr=doc.CreateAttribute(x.Name);
                    nowyAtr.Value=x.Value;
                    ret.Attributes.Append(nowyAtr);
                }
                foreach (XmlNode x in wej.ChildNodes)
                {
                    ret.AppendChild(klonujXML(doc, x));
                }
                return ret;
            }
            else return null;
        }

        internal static string sekundy(int p)
        {
            string ret="";
            var sek = (int)(p / plik.Hz);
            var min = sek / 60;
            sek = sek % 60;
            var godz = min / 60;
            min = min % 60;
            if (godz > 9)
                ret = godz.ToString() + ':';
            else if (godz > 0)
                ret = '0' + godz.ToString() + ':';
            if (min > 9)
                ret += min.ToString() + ':';
            else
                ret += '0' + min.ToString() + ':';
            if (sek > 9)
                ret += sek.ToString();
            else
                ret += '0' + sek.ToString();
            return ret;

        }
    }
}
