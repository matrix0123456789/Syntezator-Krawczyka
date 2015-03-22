using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Syntezator_Krawczyka
{
    public class sample : IPostep
    {
        public string plik="";
        public float note;
        public int częstotliwość;
        public float accept;
        public float[,] fala;
        public ushort kanały = 1;
        private string p;
        public event Action load;
        public sample(string p)
        {
            {
                plik = p;
                System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
                {
                    BinaryReader zawartość;
                    if (p.Substring(p.Length - 4) == ".mp3")
                    {
                        var bufor = new byte[1024 * 1024];
                        var read = new Mp3FileReader(p);
                        WaveStream convertedStream = WaveFormatConversionStream.CreatePcmStream(read);
                        zawartość = new BinaryReader(convertedStream);
                        kanały = (ushort)convertedStream.WaveFormat.Channels;
                        częstotliwość = convertedStream.WaveFormat.SampleRate;
                        bitrate = convertedStream.WaveFormat.BitsPerSample;

                    }
                    else
                    {
                        try
                        {
                            zawartość = new BinaryReader((new StreamReader((new Regex("\\\\([^\\\\]*)$")).Replace(Syntezator_Krawczyka.plik.URLStatyczne, "\\" + plik), Encoding.ASCII)).BaseStream);
                        }
                        catch (Exception e)
                        {
                            try
                            {
                                zawartość = new BinaryReader((new StreamReader(plik, Encoding.ASCII)).BaseStream);
                            }
                            catch (Exception e2)
                            {
                                MessageBox.Show(e2.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            zawartość.BaseStream.Position = 22;
                            kanały = zawartość.ReadUInt16();
                            częstotliwość = zawartość.ReadInt32(); if (kanały == 0 || częstotliwość == 0)

                            { MessageBox.Show("Nieprawidłowy plik", "Bląd", MessageBoxButton.OK, MessageBoxImage.Error); }
                            bitrate = 8 * zawartość.ReadInt32() / częstotliwość / kanały;
                        }
                    }


                    zawartość.BaseStream.Position = 44;
                    if (kanały == 1)
                    {
                        if (bitrate == 32)
                        {
                            fala = new float[1, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 4];
                            for (i = 0; i < fala.Length; i++)
                            {
                                fala[0, i] = zawartość.ReadInt32() / (256f * 256f * 128f);
                            }
                        }
                        if (bitrate == 16)
                        {
                            fala = new float[1, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 2];
                            for (i = 0; i < fala.Length; i++)
                            {
                                fala[0, i] = zawartość.ReadInt16() / 32768f;
                            }
                        }
                        if (bitrate == 8)
                        {
                            fala = new float[1, zawartość.BaseStream.Length - zawartość.BaseStream.Position];
                            for (i = 0; i < fala.Length; i++)
                            {
                                fala[0, i] = zawartość.ReadSByte() / 128f;
                            }
                        }
                    }
                    else
                    {
                        if (bitrate == 32)
                        {
                            fala = new float[kanały, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 4 / kanały];
                            for (i = 0; i < fala.Length / kanały; i++)
                            {
                                for (byte k = 0; k < kanały; k++)
                                    fala[k, i] = zawartość.ReadInt32() / (256f * 256f * 128f);
                            }
                        }
                        if (bitrate == 16)
                        {
                            fala = new float[kanały, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 2 / kanały];
                            for (i = 0; i < fala.Length / kanały; i++)
                            {
                                for (byte k = 0; k < kanały; k++)
                                    fala[k, i] = zawartość.ReadInt16() / 32768f;
                            }
                        }
                        if (bitrate == 8)
                        {
                            fala = new float[kanały, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / kanały];
                            for (i = 0; i < fala.Length / kanały; i++)
                            {
                                for (byte k = 0; k < kanały; k++)
                                    fala[k, i] = zawartość.ReadSByte() / 128f;
                            }
                        }
                    }
                    if (load != null)
                        load();
                });
            }
        }

        public sample(List<float[,]> bufory)
        {
            this.bufory = bufory;
            częstotliwość = 48000;
            bitrate = 16;
            kanały = (ushort)bufory[0].GetLength(0);
            long dlugosc = 0;
            for (int i = 0; i < bufory.Count; i++)
                dlugosc += bufory[i].GetLongLength(1);
            fala = new float[kanały, dlugosc];
            int przy = 0;//TODO pozbyć się zbędnego kopiowania
            for (int i = 0; i < bufory.Count; i++)
                 {for (int i2 = 0; i2 < kanały; i2++)
               
                    for (int i3 = 0; i3 < bufory[i].Length / kanały; i3++)
                    {
                        fala[i2, przy+i3] = bufory[i][i2, i3];
                        //przy++;
                    }
                    przy += bufory[i].GetLength(1);
                }
            this.bufory = null;

        }
        public long i = 0;
        private List<float[,]> bufory;
        public int bitrate { get; set; }
        public long value
        {
            get
            {
                return i;
            }
        }
        public long max
        {
            get
            {
                if (fala == null)
                    return 0;
                return fala.GetLongLength(1);
            }
        }
    }
}
