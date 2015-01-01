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
using System.Globalization;

namespace Syntezator_Krawczyka.Synteza
{
    /// <summary>
    /// Interaction logic for oscylatorUI.xaml
    /// </summary>
    public partial class oscylatorUI : UserControl
    {
        public oscylator parentNode;
        public oscylatorUI(oscylator th)
        {
            parentNode = th;
            InitializeComponent();
        }

        private void sinusoidalny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "sinusoidalna";
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void trójkątny_Checked(object sender, RoutedEventArgs e)
        {
            parentNode.ustawienia["typ"] = "trójkątna";
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void prostokątny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "prostokątna";
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void piłokształtny_Checked(object sender, RoutedEventArgs e)
        {

            parentNode.ustawienia["typ"] = "piłokształtna";
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            parentNode.ustawienia["gladkosc"] = slider1.Value.ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["A"] = (sliderA.Value).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void sliderD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["D"] = (sliderD.Value).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void sliderS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["S"] = sliderS.Value.ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }

        private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["R"] = (sliderR.Value).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }
        private void sliderBalans_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parentNode.ustawienia["balans"] = (sliderBalans.Value).ToString(CultureInfo.InvariantCulture);
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();
        }
        void ustawSuwaki()
        {

            slider1.Value = double.Parse(parentNode.ustawienia["gladkosc"], CultureInfo.InvariantCulture);
            sliderA.Value = double.Parse(parentNode.ustawienia["A"], CultureInfo.InvariantCulture);
            sliderD.Value = double.Parse(parentNode.ustawienia["D"], CultureInfo.InvariantCulture);
            sliderS.Value = double.Parse(parentNode.ustawienia["S"], CultureInfo.InvariantCulture);
            sliderR.Value = double.Parse(parentNode.ustawienia["R"], CultureInfo.InvariantCulture);
            sliderBalans.Value = double.Parse(parentNode.ustawienia["balans"], CultureInfo.InvariantCulture);
            switch(parentNode.ustawienia["typ"])
            {
                case ("sinusoidalna"):
                    sinusoidalny.IsChecked = true;
                    break;
                case ("trójkątna"):
                    trójkątny.IsChecked = true;
                    break;
                case ("prostokątna"):
                    prostokątny.IsChecked = true;
                    break;
                case ("piłokształtna"):
                    piłokształtny.IsChecked = true;
                    break;
                case ("szum"):
                    szum.IsChecked = true;
                    break;
                default:
                    niestandard.IsChecked = true;
                    break;
            }
        }
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            ustawSuwaki();
            parentNode.akt();
        }
        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (parentNode.niestandardowa!=null)
            {
                parentNode.ustawienia["typ"] = parentNode.niestandardowa.nazwa;

                parentNode.akt();
                Statyczne.otwartyplik.zmiana();
        }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EdytorFali okno;
            if(parentNode==null)
                okno = new EdytorFali();
            else
                okno = new EdytorFali(parentNode);
            okno.Show();
        }

        private void szum_Checked(object sender, RoutedEventArgs e)
        {
            parentNode.ustawienia["typ"] = "szum";
            parentNode.akt();
            Statyczne.otwartyplik.zmiana();

        }
    }
}
