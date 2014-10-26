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
        float pokażOd = 0, pokażDo;

        void rysujFala()
        {
            Fala.Children.Clear();
            var path=new Polygon();
            path.HorizontalAlignment = HorizontalAlignment.Left;
            path.Fill = Brushes.Red;
            var piksel = (pokażDo - pokażOd) / Fala.ActualWidth;
            for (var i = 0; i < Fala.ActualWidth && Math.Floor(pokażOd + i * piksel) < Dźwięk.sample.fala.GetLength(1); i++)
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
                    else if (Dźwięk.sample.fala[0, i2] <min)
                        min = Dźwięk.sample.fala[0, i2];
                }
                path.Points.Add(new Point(i, (max+1)*Fala.ActualHeight/2));
                path.Points.Insert(0, new Point(i, (min + 1) * Fala.ActualHeight / 2));
            }
            Fala.Children.Add(path);
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

            myszX = (float)e.GetPosition(this).X;
            myszY = (float)e.GetPosition(this).Y;
        }
        private void Fala_MouseUp(object sender, MouseButtonEventArgs e)
        {

            myszX = myszY = float.NaN;
        }
        private void Fala_MouseMove(object sender, MouseEventArgs e)
        {
            if (myszX != float.NaN && myszY != float.NaN && (e.LeftButton==MouseButtonState.Pressed || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                var piksel = (float)((pokażDo - pokażOd) / Fala.ActualWidth);
                var różn = ((float)e.GetPosition(this).X - myszX) * piksel;
                pokażDo -= różn;
                pokażOd -= różn;
            }

            myszX = (float)e.GetPosition(this).X;
            myszY = (float)e.GetPosition(this).Y;
            rysujFala();
        }

    }
}
