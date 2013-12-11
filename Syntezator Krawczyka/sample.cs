using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Syntezator_Krawczyka
{
    public class sample
    {
        public string plik
        {
            get
            {
                return _plik;
            }
            set
            {
                _plik = value;
                System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            {
                BinaryReader zawartość;
                try
                {
                    zawartość = new BinaryReader((new StreamReader((new Regex("\\\\([^\\\\]*)$")).Replace(Syntezator_Krawczyka.plik.URLStatyczne, "\\" + _plik), Encoding.ASCII)).BaseStream);
                }
                catch (Exception e)
                {
                    try
                    {
                        zawartość = new BinaryReader((new StreamReader(_plik, Encoding.ASCII)).BaseStream);
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString(),"Błąd",MessageBoxButton.OK,MessageBoxImage.Error);
                        return;
                    }
                }
                zawartość.BaseStream.Position = 24;
                częstotliwość = zawartość.ReadInt32();
                bitrate = 8 * zawartość.ReadInt32() / częstotliwość;

                zawartość.BaseStream.Position = 44;
                if (bitrate == 16)
                {
                    fala = new float[(zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 2];
                    for (var i = 0; i < fala.Length; i++)
                    {
                        fala[i] = zawartość.ReadInt16() / 32768f;
                    }
                }
                if (bitrate == 8)
                {
                    fala = new float[zawartość.BaseStream.Length - zawartość.BaseStream.Position];
                    for (var i = 0; i < fala.Length; i++)
                    {
                        fala[i] = zawartość.ReadSByte() / 128f;
                    }
                }
            });
            }
        }
        string _plik;
        public float note;
        public int częstotliwość;
        public float accept;
        public float[] fala;
        public int bitrate { get; set; }
    }
}
