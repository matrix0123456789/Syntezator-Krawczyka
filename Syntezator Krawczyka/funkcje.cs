﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.BassAsio;

namespace Syntezator_Krawczyka
{
    
    static class funkcje
    {

        //static byte[] puste2=null;
        static public bool graj(double[] fala, double głośność)
        {
            var bufor=new byte[fala.Length*2];
            for (var i = 0; i < fala.Length; i++)
            {
                short liczba;
                //if(fala[i]>=0)
                    liczba = (short)(fala[i] * głośność * short.MaxValue);
                //else
                    //liczba = (short)(fala[i] * głośność * -short.MaxValue);
                bufor[2 * i+1] = (byte)Math.Floor(liczba / 256f);
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
            //var c = BassAsio.BASS_ASIO_GetDeviceInfos();
            //Un4seen.BassAsio.BassAsio.BASS_ASIO_Init(0, BASSASIOInit.BASS_ASIO_THREAD);

            //Microsoft.VisualBasic.Devices.Audio iss = new Microsoft.VisualBasic.Devices.Audio();
            //iss.Play(wave(fala, głośność), Microsoft.VisualBasic.AudioPlayMode.Background);
            /*
            try
            {
                //string adr="C:\\Users\\Public\\Documents\\synteza\\wynik.wav";
                //System.IO.StreamWriter sw = new System.IO.StreamWriter(adr, false);
                //System.IO.BinaryWriter write = new System.IO.BinaryWriter(sw.BaseStream);
                //write.Write(puste);
            }
            catch { }*/


        }
        /// <summary>
        /// Zapisuje dźwięk do pliku wave
        /// </summary>
        /// <param name="fala"></param>
        /// <param name="głośność"></param>
        /// <param name="plik">ścierzka do pliku</param>
        static public void zapisz(double[] fala, double głośność, string plik)
        {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(plik, false);
                System.IO.BinaryWriter write = new System.IO.BinaryWriter(sw.BaseStream);
                var p = wave(fala, głośność);
                write.Write(p);


        }
        /// <summary>
        /// Tworzy plik wave
        /// </summary>
        /// <param name="fala"></param>
        /// <param name="głośność"></param>
        /// <returns>Tablica odpowiadająca plikowi wave</returns>
        static public byte[] wave(double[] fala, double głośność)
        {
            /*System.IO.StreamReader sa = new System.IO.StreamReader("C:\\Users\\Public\\Documents\\sinusoida.wav");
            System.IO.BinaryReader read = new System.IO.BinaryReader(sa.BaseStream);
            puste2 = read.ReadBytes((int)read.BaseStream.Length);*/
            char[] pus = Syntezator_Krawczyka.Properties.Resources.czysty.ToCharArray();
            byte[] puste = new byte[pus.Length + fala.Length * 2 - 2];
            //byte[] puste = new byte[40000];
            for (int i = 0; i < pus.Length && i < puste.Length; i++)
            {
                puste[i] = (byte)pus[i];
            }
            //for (int iss = 0; iss < 10000; iss++)
            //   sinus[rand.Next(1000, sinus.Length - 1)] = 20;
            //sinus[rand.Next(sinus.Length - 1)] = (byte)rand.Next(255);
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
            long falai = 0;
            for (int z = pus.Length - 2; z < puste.Length && fala.LongLength > falai; z = z + 2)
            {

                if (fala[falai] * głośność >= 1)
                    puste[z] = 127;
                else if (fala[falai] >= 0)
                    puste[z] = (byte)(fala[falai] * 127 * głośność);
                else if (fala[falai] * głośność > -1)
                    puste[z] = (byte)((1 + fala[falai] * głośność) * 127 + 128);
                else
                    puste[z] = 128;
                puste[z - 1] = (byte)((fala[falai] * głośność * 127 * 256) % 256);
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
            return 130.812783 * Math.Pow(2, (oktawa)+(ton / 6));
        }
        
    }
}