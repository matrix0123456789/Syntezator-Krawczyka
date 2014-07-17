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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Obramówka dla pojedyńczego „wirtuanego instrumentu”
    /// </summary>
    public partial class Instrument : UserControl
    {
        public sound parent;
        public Instrument()
        {
            InitializeComponent();
            Children=wewnętrzny.Children;
        }

        public Instrument(string p, sound s):this()
        {
            parent = s;
            if (p != null)
                label1.Content += " — "+p;
        }
        public UIElementCollection Children;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var y in parent)
            {
                modułFunkcje.zapiszXML(y.Value.ustawienia, y.Value.XML);
            }
            Clipboard.SetData("audio/x-syntezator-krawczyka-instrument", parent.xml.OwnerDocument.FirstChild.OuterXml + "<file>" + parent.xml.OuterXml + "</file>");
        }


        private void UserControl_Drag(object sender, MouseButtonEventArgs e)
        {
            foreach (var y in parent)
            {
                modułFunkcje.zapiszXML(y.Value.ustawienia, y.Value.XML);
            }
            var obj = new DataObject("audio/x-syntezator-krawczyka-instrument", parent.xml.OwnerDocument.FirstChild.OuterXml + "<file>" + parent.xml.OuterXml + "</file>");

            obj.SetData("audio/x-syntezator-krawczyka-instrument", parent.xml.OwnerDocument.FirstChild.OuterXml + "<file>" + parent.xml.OuterXml + "</file>");
            DragDrop.DoDragDrop(this, obj , DragDropEffects.All);
            MainWindow.hashCodeDragAndDrop = obj.GetData("audio/x-syntezator-krawczyka-instrument").GetHashCode();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.thi.klawiatkompa.sekw = parent.sekw;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (wewnętrzny.Visibility == Visibility.Collapsed)
            {
                wewnętrzny.Visibility = Visibility.Visible;
                (sender as Button).Content = "Zwiń";
            }else
            {
                wewnętrzny.Visibility = Visibility.Collapsed;
                (sender as Button).Content = "Rozwiń";
            }
        }

    }
}