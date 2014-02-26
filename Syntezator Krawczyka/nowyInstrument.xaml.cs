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
using System.Xml;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Interaction logic for nowyInstrument.xaml
    /// </summary>
    public partial class nowyInstrument : Window
    {
        public nowyInstrument()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            laduj(Properties.Resources.przyklad);
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            laduj(Properties.Resources.przyklad2);
        }
        void laduj(String plik)
        {

            if (Statyczne.otwartyplik != null)
            {

                var xml = new XmlDocument();
                xml.LoadXml(plik);
                var soundList = xml.GetElementsByTagName("sound");
                var sound = soundList[0];
                var id = sound.Attributes.GetNamedItem("id").Value;
                for (var i = 1; true; i++)
                {
                    if (!Statyczne.otwartyplik.moduły.ContainsKey(id + i.ToString()))
                    {
                        id = id + i.ToString();
                        break;
                    }

                }
                sound.Attributes.GetNamedItem("id").Value = id;
                var klon = funkcje.klonujXML(Statyczne.otwartyplik.xml, sound);
                Statyczne.otwartyplik.xml.GetElementsByTagName("file")[0].AppendChild(klon);

                Statyczne.otwartyplik.dekoduj();//poprawić na nową referencję
                Close();
            }
        }
    }
}
