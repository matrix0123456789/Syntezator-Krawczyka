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
        public string plik;
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
                    if(p.Substring(p.Length-4)==".mp3")
                    {
                        var bufor=new byte[1024*1024];
                        var read = new Mp3FileReader(p);
                        WaveStream convertedStream = WaveFormatConversionStream.CreatePcmStream(read);
                        zawartość = new BinaryReader(convertedStream);
                        kanały = (ushort)convertedStream.WaveFormat.Channels;
                        częstotliwość = convertedStream.WaveFormat.SampleRate;
                        bitrate = convertedStream.WaveFormat.BitsPerSample;
                        
                    }else{
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
                    }}
                   

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
                            fala = new float[kanały, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 4/kanały];
                            for (i = 0; i < fala.Length / kanały; i++)
                            {
                                for (byte k = 0; k < kanały;k++ )
                                    fala[k, i] = zawartość.ReadInt32() / (256f * 256f * 128f);
                            }
                        }
                        if (bitrate == 16)
                        {
                            fala = new float[kanały, (zawartość.BaseStream.Length - zawartość.BaseStream.Position) / 2/kanały];
                            for (i = 0; i < fala.Length/kanały; i++)
                            {
                                for (byte k = 0; k < kanały; k++)
                                fala[k, i] = zawartość.ReadInt16() / 32768f;
                            }
                        }
                        if (bitrate == 8)
                        {
                            fala = new float[kanały,( zawartość.BaseStream.Length - zawartość.BaseStream.Position)/kanały];
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
        long i = 0;
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
