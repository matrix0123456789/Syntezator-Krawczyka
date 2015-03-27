using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Syntezator_Krawczyka
{
    public class sample : IPostep
    {
        public string plik = "";
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
                    else if (p.Substring(p.Length - 4) == ".wav" || p.Substring(p.Length - 5) == ".wave")
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
                    else
                    {
                      /*  IMFSourceReader pReader;
                        MediaFoundationInterop.MFCreateSourceReaderFromURL(p, null, out pReader);
                        pReader.SetStreamSelection(MediaFoundationInterop.MF_SOURCE_READER_ALL_STREAMS, false);
                        pReader.SetStreamSelection(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);
                        IMFMediaType partialMediaType;
                        MediaFoundationInterop.MFCreateMediaType(out partialMediaType);
                        partialMediaType.SetGUID(MediaFoundationAttributes.MF_MT_MAJOR_TYPE, MediaTypes.MFMediaType_Audio);
                        partialMediaType.SetGUID(MediaFoundationAttributes.MF_MT_SUBTYPE, AudioSubtypes.MFAudioFormat_PCM);
                        pReader.SetCurrentMediaType(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, IntPtr.Zero, partialMediaType);
                        Marshal.ReleaseComObject(partialMediaType); IMFMediaType uncompressedMediaType;
                        pReader.GetCurrentMediaType(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM,
                                out uncompressedMediaType);
                        Guid audioSubType;
                        uncompressedMediaType.GetGUID(MediaFoundationAttributes.MF_MT_SUBTYPE, out audioSubType);
                        Debug.Assert(audioSubType == AudioSubtypes.MFAudioFormat_PCM);
                        int channels;
                        uncompressedMediaType.GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_NUM_CHANNELS, out channels);
                        int bits;
                        uncompressedMediaType.GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_BITS_PER_SAMPLE, out bits);
                        int sampleRate;
                        uncompressedMediaType.GetUINT32(MediaFoundationAttributes.MF_MT_AUDIO_SAMPLES_PER_SECOND, out sampleRate);
                       var waveFormat = new WaveFormat(sampleRate, bits, channels);

                       IMFSample pSample;
                       int dwFlags;
                       ulong timestamp;
                       int actualStreamIndex;
                       pReader.ReadSample(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0, out actualStreamIndex, out dwFlags, out timestamp, out pSample);
                       if (dwFlags != 0) // reached the end of the stream or media type changed
                       {
                           return;
                       } */

                        var bufor = new byte[1024 * 1024];
                        
                        var read = new MediaFoundationReader(p);
                        WaveStream convertedStream = WaveFormatConversionStream.CreatePcmStream(read);
                        zawartość = new BinaryReader(convertedStream);
                        kanały = (ushort)convertedStream.WaveFormat.Channels;
                        częstotliwość = convertedStream.WaveFormat.SampleRate;
                        bitrate = convertedStream.WaveFormat.BitsPerSample;
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
            {
                for (int i2 = 0; i2 < kanały; i2++)

                    for (int i3 = 0; i3 < bufory[i].Length / kanały; i3++)
                    {
                        fala[i2, przy + i3] = bufory[i][i2, i3];
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
