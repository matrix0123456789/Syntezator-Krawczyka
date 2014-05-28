using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Xml;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for EdytorNut.xaml
    /// </summary>
    public partial class EdytorNut : Window
    {
        sciezka main;
        const double skalaX = 20;
        const double skalaY = 20;
        double tonMax;
        double tonMin;
        public EdytorNut(sciezka input)
        {
            InitializeComponent();
            main = input;
            if (input.nuty.Count > 0)
            {
                double ilepróbekMin, ilepróbekMax;
                ilepróbekMax = ilepróbekMin = input.nuty[0].ilepróbekNaStarcie;
                foreach (var x in input.nuty)
                {
                    if (x.ilepróbekNaStarcie > ilepróbekMax)
                    {
                        ilepróbekMax = x.ilepróbekNaStarcie;
                    }
                    else if (x.ilepróbekNaStarcie < ilepróbekMin)
                    {
                        ilepróbekMin = x.ilepróbekNaStarcie;
                    }
                }
                // double tonMin=funkcje.ton(ilepróbekMin)-funkcje.ton(ilepróbekMax)
                tonMin = funkcje.ton(ilepróbekMin) + 1;
                tonMax = funkcje.ton(ilepróbekMax) - 1;
                for (var i = tonMin; i >= tonMax; i = i - .5)
                {
                    var tonTeraz = Math.Round((i + 600) % 6, 2);//+600 żeby dla ujemnych nie sprawdzał w odwrotnej kolejności
                    if (tonTeraz == .5 || tonTeraz == 1.5 || tonTeraz == 3 || tonTeraz == 4 || tonTeraz == 5)
                    {
                        var prostokat = new Rectangle();
                        prostokat.Margin = new Thickness(0, (tonMin - i) * skalaY, 0, 0);
                        prostokat.Height = (skalaY / 2);
                        prostokat.Fill = Brushes.Beige;
                        prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        panel.Children.Add(prostokat);
                    }
                    else if (tonTeraz == 0)//oktawa
                    {
                        var prostokat = new Rectangle();
                        prostokat.Margin = new Thickness(0, (tonMin - i) * skalaY, 0, 0);
                        prostokat.Height = (1);
                        prostokat.Fill = Brushes.Black;
                        prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        panel.Children.Add(prostokat);
                    }
                }
                for (int i = 0; i < input.nuty.Count; i++)
                {
                    var prostokat = new Rectangle();
                    prostokat.Margin = new Thickness((plik.tempo * (input.nuty[i].opuznienie - input.delay) / (60 * plik.Hz) * skalaX), (tonMin - funkcje.ton(input.nuty[i].ilepróbekNaStarcie)) * skalaY, 0, 0);

                    prostokat.Width = plik.tempo * input.nuty[i].długość / (60 * plik.Hz) * skalaX;
                    prostokat.Height = (skalaY / 2);
                    prostokat.Fill = Brushes.Red;
                    prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    prostokat.Tag = new nutaXml(input.nuty[i], input.xml.ChildNodes[i]);
                    prostokat.MouseDown += prostokat_MouseClick;
                    panel.Children.Add(prostokat);
                }
            }
        }

        void prostokat_MouseClick(object sender, MouseEventArgs e)
        {
            if (aktywna != null)
                aktywna.Stroke = null;
            aktywna = (Rectangle)sender;

            var nuta = (nutaXml)((sender as Rectangle).Tag);
            czas.IsEnabled = dlugosc.IsEnabled = ton.IsEnabled = true;
            aktywna.Stroke = Brushes.Green;

            try
            {
                czas.Text = double.Parse(nuta.xml.Attributes.GetNamedItem("delay").Value, CultureInfo.InvariantCulture).ToString();
            }
            catch (NullReferenceException)
            {
                czas.Text = "0";
            }
            try
            {
                dlugosc.Text = double.Parse(nuta.xml.Attributes.GetNamedItem("duration").Value, CultureInfo.InvariantCulture).ToString();
            }
            catch (NullReferenceException)
            {
                dlugosc.Text = "0";
            }
            try
            {
                ton.Text = (double.Parse(nuta.xml.Attributes.GetNamedItem("note").Value, CultureInfo.InvariantCulture) + 6 * double.Parse(nuta.xml.Attributes.GetNamedItem("octave").Value, CultureInfo.InvariantCulture)).ToString();
            }
            catch (NullReferenceException)
            {
                ton.Text = "0";
            }
        }

        public Rectangle aktywna { get; set; }

        private void czas_TextChanged(object sender, TextChangedEventArgs e)
        {

            usuńLitery((TextBox)sender);
            if (aktywna != null)
            {
                try
                {
                    var n = (nutaXml)aktywna.Tag;
                    var cz = double.Parse(czas.Text);
                    var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("delay");
                    atrybut.Value = cz.ToString(CultureInfo.InvariantCulture);
                    n.xml.Attributes.SetNamedItem(atrybut);
                    aktywna.Margin = new Thickness((cz * skalaX), aktywna.Margin.Top, 0, 0);
                    n.nuta.opuznienie = (long)(plik.Hz * 60 / plik.tempo * cz);
                    (sender as TextBox).Background = Brushes.White;
                }
                catch (FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
            }
        }
        private void dlugosc_TextChanged(object sender, TextChangedEventArgs e)
        {
            usuńLitery((TextBox)sender);
            if (aktywna != null)
            {
                try
                {
                    var n = (nutaXml)aktywna.Tag;
                    var cz = float.Parse(dlugosc.Text);
                    var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("duration");
                    atrybut.Value = cz.ToString(CultureInfo.InvariantCulture);
                    n.xml.Attributes.SetNamedItem(atrybut);
                    aktywna.Width = cz * skalaY;
                    n.nuta.długość = (long)(plik.Hz * 60 / plik.tempo * cz);
                    (sender as TextBox).Background = Brushes.White;
                }
                catch (FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
            }
        }
        struct nutaXml
        {
            public nuta nuta;
            public XmlNode xml;
            public nutaXml(nuta a, XmlNode b)
            {
                nuta = a;
                xml = b;
            }
        }

        private void ton_TextChanged(object sender, TextChangedEventArgs e)
        {
            usuńLitery((TextBox)sender);
            if (aktywna != null)
            {
                try
                {
                    var n = (nutaXml)aktywna.Tag;
                    var cz = float.Parse(ton.Text);
                    var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("note");
                    atrybut.Value = cz.ToString(CultureInfo.InvariantCulture);
                    n.xml.Attributes.SetNamedItem(atrybut);
                    var atrybut2 = Statyczne.otwartyplik.xml.CreateAttribute("octave");
                    atrybut2.Value = "0";
                    n.xml.Attributes.SetNamedItem(atrybut2);
                    aktywna.Margin = new Thickness(aktywna.Margin.Left, (tonMin - cz) * skalaY, 0, 0);
                    (sender as TextBox).Background = Brushes.White;
                    n.nuta.ilepróbekNaStarcie = n.nuta.ilepróbek = funkcje.ilepróbek(0, cz);
                }
                catch (FormatException)
                {
                    (sender as TextBox).Background = Brushes.Red;
                }
            }
        }

        private void usuńLitery(TextBox textBox)
        {
            if ((textBox.Text[0] >= 'a' && textBox.Text[0] <= 'z') || (textBox.Text[0] >= 'A' && textBox.Text[0] <= 'Z'))
            {
                textBox.Text = textBox.Text.Substring(1);
                textBox.Select(textBox.Text.Length, 0);
            } if ((textBox.Text[textBox.Text.Length - 1] >= 'a' && textBox.Text[textBox.Text.Length - 1] <= 'z') || (textBox.Text[textBox.Text.Length - 1] >= 'A' && textBox.Text[textBox.Text.Length - 1] <= 'Z'))
            {
                textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
                textBox.Select(textBox.Text.Length, 0);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Przycisk "nowa nuta"
        {
            var nuta = new nuta(funkcje.częstotliwość(0, 0), (long)plik.Hz * 60 / (long)plik.tempo, main.delay);
            main.nuty.Add(nuta);
            var nutaXML = Statyczne.otwartyplik.xml.CreateElement("nute");

            var atrybut = Statyczne.otwartyplik.xml.CreateAttribute("note");
            atrybut.Value = "0";
            nutaXML.Attributes.SetNamedItem(atrybut);
            var atrybut2 = Statyczne.otwartyplik.xml.CreateAttribute("octave");
            atrybut2.Value = "0";
            nutaXML.Attributes.SetNamedItem(atrybut2);
            var atrybut3 = Statyczne.otwartyplik.xml.CreateAttribute("delay");
            atrybut3.Value = "0";
            nutaXML.Attributes.SetNamedItem(atrybut3);
            var atrybut4 = Statyczne.otwartyplik.xml.CreateAttribute("duration");
            atrybut4.Value = "1";
            nutaXML.Attributes.SetNamedItem(atrybut4);

            main.xml.AppendChild(nutaXML);
            var prostokat = new Rectangle();
            prostokat.Margin = new Thickness(0, (tonMin) * skalaY, 0, 0);

            prostokat.Width = skalaX;
            prostokat.Height = (skalaY / 2);
            prostokat.Fill = Brushes.Red;
            prostokat.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            prostokat.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            prostokat.Tag = new nutaXml(nuta, nutaXML);
            prostokat.MouseDown += prostokat_MouseClick;
            panel.Children.Add(prostokat);
            prostokat_MouseClick(prostokat, null);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    nowaNuta.Focus();
                    break;
                case Key.C:
                    czas.Focus();
                    
                    break;
                case Key.D:
                    dlugosc.Focus();
                    break;
                case Key.W:
                    ton.Focus();
                    break;
                case Key.N:
                    Button_Click(null, null);
                    break;
                case Key.Right:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                        if (aktywna == null)
                        {
                            if (panel.Children.Count > 0)
                                prostokat_MouseClick(panel.Children[0], null);
                        }
                        else
                        {
                            var nr = panel.Children.IndexOf(aktywna);
                            if (panel.Children.Count > nr+1)
                                prostokat_MouseClick(panel.Children[nr + 1], null);

                        }
                    break;
                case Key.Left:
                    if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                        if (aktywna == null)
                        {
                            if (panel.Children.Count > 0)
                                prostokat_MouseClick(panel.Children[0], null);
                        }
                        else
                        {
                            var nr = panel.Children.IndexOf(aktywna);
                            if (nr > 0)
                                prostokat_MouseClick(panel.Children[nr - 1], null);

                        }
                    break;
            }
        }
    }
}
