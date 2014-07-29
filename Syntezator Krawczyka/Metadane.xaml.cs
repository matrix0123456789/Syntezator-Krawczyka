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
    /// Interaction logic for Metadane.xaml
    /// </summary>
    public partial class Metadane : Window
    {
        public Metadane()
        {
            InitializeComponent();
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("name").Count > 0)
            {
                tytul.Text = Statyczne.otwartyplik.xml.GetElementsByTagName("name")[0].InnerText;
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("album").Count > 0)
            {
                album.Text = Statyczne.otwartyplik.xml.GetElementsByTagName("album")[0].InnerText;
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("author").Count > 0)
            {
                autor.Text = Statyczne.otwartyplik.xml.GetElementsByTagName("author")[0].InnerText;
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("description").Count > 0)
            {
                opis.Text = Statyczne.otwartyplik.xml.GetElementsByTagName("description")[0].InnerText;
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("year").Count > 0)
            {
                rok.Text = Statyczne.otwartyplik.xml.GetElementsByTagName("year")[0].InnerText;
            }
        }


        private void Zapisz_click(object sender, RoutedEventArgs e)
        {
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("name").Count > 0)
            {
                Statyczne.otwartyplik.xml.GetElementsByTagName("name")[0].InnerText = tytul.Text;
            }
            else
            {
                var el = Statyczne.otwartyplik.xml.CreateElement("name");
                el.InnerText = tytul.Text;
                Statyczne.otwartyplik.xml.DocumentElement.AppendChild(el);
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("album").Count > 0)
            {
                Statyczne.otwartyplik.xml.GetElementsByTagName("album")[0].InnerText = album.Text;
            }
            else
            {
                var el = Statyczne.otwartyplik.xml.CreateElement("album");
                el.InnerText = album.Text;
                Statyczne.otwartyplik.xml.DocumentElement.AppendChild(el);
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("author").Count > 0)
            {
                Statyczne.otwartyplik.xml.GetElementsByTagName("author")[0].InnerText = autor.Text;
            }
            else
            {
                var el = Statyczne.otwartyplik.xml.CreateElement("author");
                el.InnerText = autor.Text;
                Statyczne.otwartyplik.xml.DocumentElement.AppendChild(el);
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("description").Count > 0)
            {
                Statyczne.otwartyplik.xml.GetElementsByTagName("description")[0].InnerText = opis.Text;
            }
            else
            {
                var el = Statyczne.otwartyplik.xml.CreateElement("description");
                el.InnerText = opis.Text;
                Statyczne.otwartyplik.xml.DocumentElement.AppendChild(el);
            }
            if (Statyczne.otwartyplik.xml.GetElementsByTagName("year").Count > 0)
            {
                Statyczne.otwartyplik.xml.GetElementsByTagName("year")[0].InnerText = rok.Text;
            }
            else
            {
                var el = Statyczne.otwartyplik.xml.CreateElement("year");
                el.InnerText = rok.Text;
                Statyczne.otwartyplik.xml.DocumentElement.AppendChild(el);
            }
        }

        private void Ok_click(object sender, RoutedEventArgs e)
        {
            Zapisz_click(null, null);
            Close();
        }
    }
}
