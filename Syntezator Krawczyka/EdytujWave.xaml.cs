using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for EdytujWave.xaml
    /// </summary>
    public partial class EdytujWave : Window
    {
        private jedenSample Dźwięk;
        public EdytujWave(jedenSample j)
        {
            InitializeComponent();
            Dźwięk = j;
            pokażDo = j.dlugosc;
            rysujFala();
        }
        float[,] temp = null;
        float pokażOd = 0, pokażDo;
        const int mnożnikTemp = 2048;
        void rysujFala()
        {
            // Fala.Children.Clear();
            var path = (Polygon)Fala.Children[1];
            path.Points.Clear();
            path.HorizontalAlignment = HorizontalAlignment.Left;
            path.Fill = Brushes.Red;
            var jakosc = 1;
            var piksel = (pokażDo - pokażOd) / Fala.ActualWidth * jakosc;
            var iZwiększ = (int)Math.Ceiling(Fala.ActualWidth / (pokażDo - pokażOd));
            if (iZwiększ < 1)
                iZwiększ = 1;
            if (piksel > mnożnikTemp)
            {
                if (temp == null)
                    genTemp();
                for (var i = 0; i < Fala.ActualWidth && Math.Floor(pokażOd + i * piksel) < Dźwięk.sample.fala.GetLength(1); i += iZwiększ)
                {
                    var jestW = pokażOd + i * piksel;
                    if (jestW < 0)
                        continue;
                    float min, max;
                    min = max = Dźwięk.sample.fala[0, (int)Math.Floor(jestW)];
                    for (var i2 = (int)Math.Floor(jestW); i2 < Math.Floor(jestW + piksel) && i2 < Dźwięk.sample.fala.GetLength(1); i2 += mnożnikTemp)
                    {
                        if (temp[i2 / mnożnikTemp, 0] > max)
                            max = temp[i2 / mnożnikTemp, 0];
                        else if (temp[i2 / mnożnikTemp, 0] < min)
                            min = temp[i2 / mnożnikTemp, 0];
                        if (temp[i2 / mnożnikTemp, 1] > max)
                            max = temp[i2 / mnożnikTemp, 1];
                        else if (temp[i2 / mnożnikTemp, 1] < min)
                            min = temp[i2 / mnożnikTemp, 1];
                    }
                    path.Points.Add(new Point(i * jakosc, (max + 1) * Fala.ActualHeight / 2));
                    path.Points.Insert(0, new Point(i * jakosc, (min + 1) * Fala.ActualHeight / 2 - 1));
                }
            }
            else

                for (var i = 0; i < Fala.ActualWidth && Math.Floor(pokażOd + i * piksel) < Dźwięk.sample.fala.GetLength(1); i += iZwiększ)
                {
                    var jestW = pokażOd + i * piksel;
                    if (jestW < 0)
                        continue;
                    float min, max;
                    min = max = Dźwięk.sample.fala[0, (int)Math.Floor(jestW)];
                    for (var i2 = (int)Math.Floor(jestW); i2 < Math.Floor(jestW + piksel) && i2 < Dźwięk.sample.fala.GetLength(1); i2++)
                    {
                        if (Dźwięk.sample.fala[0, i2] > max)
                            max = Dźwięk.sample.fala[0, i2];
                        else if (Dźwięk.sample.fala[0, i2] < min)
                            min = Dźwięk.sample.fala[0, i2];
                    }
                    path.Points.Add(new Point(i * jakosc, (max + 1) * Fala.ActualHeight / 2));
                    path.Points.Insert(0, new Point(i * jakosc, (min + 1) * Fala.ActualHeight / 2 - 1));
                }
            //Fala.Children.Add(path);
            var startTrj = -(pokażOd - Dźwięk.start) / piksel;
            trójkąt1.Margin = new Thickness(startTrj, 35, 0, 0);
            var endTrj = -(pokażOd - Dźwięk.end) / piksel;
            trójkąt2.Margin = new Thickness(endTrj, 35, 0, 0);

        }

        private void genTemp()
        {
            int dl = Dźwięk.sample.fala.GetLength(1);
            temp = new float[dl / mnożnikTemp + 1, 2];
            for (long i = 0; i + mnożnikTemp < dl; i += mnożnikTemp)
            {
                float min = Dźwięk.sample.fala[0, i];
                float max = min;
                for (long i2 = 1; i2 < mnożnikTemp; i2++)
                {
                    if (Dźwięk.sample.fala[0, i + i2] > max)
                        max = Dźwięk.sample.fala[0, i + i2];
                    else if (Dźwięk.sample.fala[0, i + i2] < min)
                        min = Dźwięk.sample.fala[0, i + i2];
                }
                temp[i / mnożnikTemp, 0] = min;
                temp[i / mnożnikTemp, 1] = max;
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            rysujFala();
        }

        private void minus_click(object sender, RoutedEventArgs e)
        {
            pokażDo = (float)(pokażOd + (pokażDo - pokażOd) * Math.Sqrt(2));
            rysujFala();

        }

        private void plus_click(object sender, RoutedEventArgs e)
        {
            pokażDo = (float)(pokażOd + (pokażDo - pokażOd) / Math.Sqrt(2));
            rysujFala();
        }
        float myszX = float.NaN, myszY = float.NaN;
        private void Fala_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == trójkąt1) lastDrag = 1;
            else if (e.OriginalSource == trójkąt2) lastDrag = 2;
            else lastDrag = 0;
            myszX = (float)e.GetPosition(this).X;
            myszY = (float)e.GetPosition(this).Y;
        }
        private void Fala_MouseDowntrj1(object sender, MouseButtonEventArgs e)
        {
            lastDrag = 1;
            myszX = (float)e.GetPosition(this).X;
            myszY = (float)e.GetPosition(this).Y;
        }
        private void Fala_MouseDowntrj2(object sender, MouseButtonEventArgs e)
        {
            lastDrag = 2;
            myszX = (float)e.GetPosition(this).X;
            myszY = (float)e.GetPosition(this).Y;
        }
        private void Fala_MouseUp(object sender, MouseButtonEventArgs e)
        {

            myszX = myszY = float.NaN;
        }
        byte lastDrag = 0;
        private void Fala_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastDrag == 0)
            {
                if (myszX != float.NaN && myszY != float.NaN && (e.LeftButton == MouseButtonState.Pressed || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    var piksel = (float)((pokażDo - pokażOd) / Fala.ActualWidth);
                    var różn = ((float)e.GetPosition(this).X - myszX) * piksel;
                    pokażDo -= różn;
                    pokażOd -= różn;
                }
            }
            else if (lastDrag == 1)
            {
                if (myszX != float.NaN && myszY != float.NaN && (e.LeftButton == MouseButtonState.Pressed || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    var piksel = (float)((pokażDo - pokażOd) / Fala.ActualWidth);
                    var różn = ((float)e.GetPosition(this).X - myszX) * piksel;
                    Dźwięk.start += (long)różn;
                    var atr = Dźwięk.xml.OwnerDocument.CreateAttribute("start");
                    atr.Value = Dźwięk.start.ToString();
                    Dźwięk.xml.Attributes.Append(atr);
                    Dźwięk.zmienionoDługość();
                }

            }
            else if (lastDrag == 2)
            {
                if (myszX != float.NaN && myszY != float.NaN && (e.LeftButton == MouseButtonState.Pressed || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    var piksel = (float)((pokażDo - pokażOd) / Fala.ActualWidth);
                    var różn = ((float)e.GetPosition(this).X - myszX) * piksel;
                    Dźwięk.end += (long)różn;
                    var atr = Dźwięk.xml.OwnerDocument.CreateAttribute("end");
                    atr.Value = Dźwięk.end.ToString();
                    Dźwięk.xml.Attributes.Append(atr);
                    Dźwięk.zmienionoDługość();
                }

            }

            myszX = (float)e.GetPosition(this).X;
            myszY = (float)e.GetPosition(this).Y;
            rysujFala();
        }

        private void Play_click(object sender, RoutedEventArgs e)
        {
            Dźwięk.działaj();
        }

    }
}
