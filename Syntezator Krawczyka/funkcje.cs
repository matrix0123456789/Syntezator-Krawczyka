using Syntezator_Krawczyka.Synteza;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

//reference Nuget Package NAudio.Lame
using NAudio.Wave;
using NAudio.Lame;

namespace Syntezator_Krawczyka
{

    static class funkcje
    {

        public static int liczWejścia(this moduł m)
        {
            if (m.GetType() == typeof(sekwencer))
                return 1;
            int l = 0;
            foreach (var x in m.wejście)
            {
                l+=x.DrógiModół.liczWejścia();
            }
            return l;
        }
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
            if (Oscyloskop.oscyl != null)
                Oscyloskop.dane.Enqueue(fala);
            if (Widmo.wid != null)
                Widmo.dane.Enqueue(fala);
            var bufor = new byte[fala.Length * 2];
            for (var i = 0; i < fala.Length / 2; i++)
            {
                if (fala[0, i] > 1)
                    fala[0, i] = 1;
                else if (fala[0, i] < -1)
                    fala[0, i] = -1;
                if (fala[1, i] > 1)
                    fala[1, i] = 1;
                else if (fala[1, i] < -1)
                    fala[1, i] = -1;
                short liczba;
                liczba = (short)(fala[0, i] * 32766);
                bufor[4 * i + 1] = (byte)Math.Floor(liczba / 256f);
                bufor[4 * i] = (byte)(liczba % 256);
                liczba = (short)(fala[1, i] * short.MaxValue);
                bufor[4 * i + 3] = (byte)Math.Floor(liczba / 256f);
                bufor[4 * i + 2] = (byte)(liczba % 256);
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
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(plik, false);
                System.IO.BinaryWriter write = new System.IO.BinaryWriter(sw.BaseStream);
                if (plik.Substring(plik.Length - 4) == ".mp3")
                    mp3(fala, write);
                else
                    wave(fala, write);

                write.Flush();
                write.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Błąd podczas zapisu", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                try
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(plik, false);
                    System.IO.BinaryWriter write = new System.IO.BinaryWriter(sw.BaseStream);
                    wave(fala, write);
                    write.Flush();
                    write.Close();
                }
                catch { }
            }



        }

        static public void mp3(float[,] fala, BinaryWriter writer)
        {
           
            var format = new WaveFormat(48000, 16, 2);
           Stream str = new MemoryStream();
            wave(fala, new BinaryWriter(str));
           using(  var mp3 = new LameMP3FileWriter(writer.BaseStream, format, 128000))
           using( var wav = WaveFormatConversionStream.CreatePcmStream(new RawSourceWaveStream(str, format)))
            wav.CopyTo(mp3);
        }
        /// <summary>
        /// Tworzy plik wave
        /// </summary>
        /// <param name="writer">Strumień do zapisu</param>
        /// <param name="fala">fala dżwiękowa stereo</param>
        /// <returns>Tablica odpowiadająca plikowi wave</returns>

        static public void wave(float[,] fala, BinaryWriter writer)
        {



            char[] pus = Syntezator_Krawczyka.Properties.Resources.czysty.ToCharArray();
            byte[] puste = new byte[pus.Length - 1];
            var pusteLength = pus.Length + fala.Length * (granie.bity / 8) - 2;
            for (int i = 0; i < pus.Length && i < puste.Length; i++)
            {
                puste[i] = (byte)pus[i];
            }
            byte[] rozmiar = BitConverter.GetBytes(pusteLength - 8);
            puste[4] = rozmiar[0];
            puste[5] = rozmiar[1];
            puste[6] = rozmiar[2];
            puste[7] = rozmiar[3];
            byte[] rozmiar2 = BitConverter.GetBytes(fala.Length * granie.bity / 8);
            puste[40] = rozmiar2[0];
            puste[41] = rozmiar2[1];
            puste[42] = rozmiar2[2];
            puste[43] = rozmiar2[3];


            byte[] czestotliwosc = BitConverter.GetBytes((int)plik.Hz);
            puste[24] = czestotliwosc[0];
            puste[25] = czestotliwosc[1];
            puste[26] = czestotliwosc[2];
            puste[27] = czestotliwosc[3];
            byte[] czestotliwosc2 = BitConverter.GetBytes((int)plik.Hz * 2 * granie.bity / 8);
            puste[28] = czestotliwosc2[0];
            puste[29] = czestotliwosc2[1];
            puste[30] = czestotliwosc2[2];
            puste[31] = czestotliwosc2[3];
            puste[32] = (byte)(2 * granie.bity / 8);
            puste[34] = granie.bity;



            puste[22] = 2;

            writer.Write(puste);

            for (long falai = 0; fala.LongLength / 2 > falai; falai++)
            {

                if (fala[0, falai] > 1)
                    fala[0, falai] = 1;
                else if (fala[0, falai] < -1)
                    fala[0, falai] = -1;
                if (fala[1, falai] > 1)
                    fala[1, falai] = 1;
                else if (fala[1, falai] < -1)
                    fala[1, falai] = -1;

                /* writer.Write((byte)((fala[0, falai] * 128 * 256) % 256));
                 if (fala[0, falai] >= 0)
                     writer.Write((byte)(fala[0, falai] * 128));
                 else
                     writer.Write((byte)((1 + fala[0, falai]) * 128 + 128));
                 writer.Write((byte)((fala[1, falai] * 128 * 256) % 256));
                 if (fala[1, falai] >= 0)
                     writer.Write((byte)(fala[1, falai] * 128));
                 else
                     writer.Write((byte)((1 + fala[1, falai]) * 128 + 128));*/
                if (granie.bity == 8)
                {
                    writer.Write((byte)(fala[0, falai] * 127 + 127));
                    writer.Write((byte)(fala[1, falai] * 127 + 127));
                }
                else if (granie.bity == 16)
                {
                    writer.Write((short)(fala[0, falai] * 32767));
                    writer.Write((short)(fala[1, falai] * 32767));
                }
                /*else if (granie.bity == 24)
                {
                    writer.Write((int)(fala[0, falai] * (32767)));
                    writer.Write((int)(fala[1, falai] * (32767)));
                }*/
                else if (granie.bity == 32)
                {
                    writer.Write((int)(fala[0, falai] * (32767)));
                    writer.Write((int)(fala[1, falai] * (32767)));
                }
                //puste[z + 1] = 0;


            }
            //return puste;
        }

        // const double bazowa = 130.812783;
        const double bazowa = 130;


        /// <summary>
        /// Wylicza częstotliwość nuty na podstawie oktawy i tonu
        /// </summary>
        /// <param name="oktawa"></param>
        /// <param name="ton">Ilość całych tonów powyrzej C (C to 0)</param>
        /// <returns>Częstotliwość w hercach</returns>
        public static double częstotliwość(short oktawa, float ton)
        {
            return bazowa * Math.Pow(2, (oktawa) + (ton / 6));
        }
        public static double ilepróbek(short oktawa, float ton)
        {
            return plik.Hz / (bazowa * Math.Pow(2, (oktawa) + (ton / 6)));
        }
        public static double ton(double ileprobek)
        {
            return Math.Log(plik.Hz / (bazowa * ileprobek), 2) * 6;
        }

        public static XmlNode klonujXML(XmlDocument doc, System.Xml.XmlNode wej)
        {
            if (wej.NodeType == XmlNodeType.Element)
            {
                var ret = doc.CreateElement(wej.Name);
                foreach (XmlAttribute x in wej.Attributes)
                {
                    XmlAttribute nowyAtr = doc.CreateAttribute(x.Name);
                    nowyAtr.Value = x.Value;
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
            string ret = "";
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


        public static void ConvertWavStreamToMp3File(ref MemoryStream ms, string savetofilename)
        {

        }
    }
}