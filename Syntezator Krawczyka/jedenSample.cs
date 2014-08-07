﻿using Syntezator_Krawczyka.Synteza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    class jedenSample
    {
        public sample sample;
        public float głośność = 1;
        long opuznienie;
        public jedenSample()
        {
            granie.graniestart();
        }

        internal void działaj()
        {

            float[,] dane;
            int dl;
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
                for (byte k = 0; k < sample.kanały; k++)
                    for (var i2 = 0; l > i2; i2++)
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


            if (granie.wynik == null) { }
            /*lock (granie.grają)
            {
                if (granie.grają.ContainsKey(input.id))
                {
                    grają[input.id].dźwięk = (input).dane;
                    grają[input.id].nuta = input;
                }
                else
                    grają.Add(input.id, new gra(input));
            }*/
            else
            {


                long i = opuznienie;
                var opt1 = -opuznienie;

                var opt3 = dl - opt1;
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

    }
}
