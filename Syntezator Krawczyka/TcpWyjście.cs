using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Syntezator_Krawczyka.Synteza;

namespace Syntezator_Krawczyka
{
    class TcpWyjście
    {
        public static void start()
        {
            var uListener = new System.Net.Sockets.UdpClient(1755);
            uListener.BeginReceive(czytajUdp, null);
            var listener = new System.Net.Sockets.TcpListener(1755);
            listener.Start();
            while (true)
            {
                var klient = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem((a) => { try { czytajStart(klient); } catch { } });
            }
        }

        private static void czytajStart(System.Net.Sockets.TcpClient klient)
        {
            var str = klient.GetStream();
            var read = new StreamReader(str);
            var linia = read.ReadLine();
            if (linia != null && linia.Contains("/output.wav"))
            {

                var write = new StreamWriter(str);
                write.WriteLine("HTTP/1.1 200 OK");
                write.WriteLine("content-type: audio/wav");
                write.WriteLine("");
                write.Flush();
                var falaLength = 0;
                char[] pus = Syntezator_Krawczyka.Properties.Resources.czysty.ToCharArray();
                byte[] puste = new byte[44];
                var pusteLength = pus.Length + falaLength * (granie.bity / 8) - 2;
                for (int i = 0; i < pus.Length && i < puste.Length; i++)
                {
                    puste[i] = (byte)pus[i];
                }
                byte[] rozmiar = BitConverter.GetBytes(pusteLength - 8);
                puste[4] = rozmiar[0];
                puste[5] = rozmiar[1];
                puste[6] = rozmiar[2];
                puste[7] = rozmiar[3];
                byte[] rozmiar2 = BitConverter.GetBytes(falaLength * granie.bity / 8);
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
                str.Write(puste, 0, 44);
                str.Flush();

                granie.strumienieWyjścia.Add(str);
            }
        }

        public static void czytajUdp(IAsyncResult e)
        {

        }
    }
}
