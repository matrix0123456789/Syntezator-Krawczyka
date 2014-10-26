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
    /// Interaction logic for suwakOkno.xaml
    /// </summary>
    public partial class suwakOkno : Window
    {
        private suwak suwak;

       

        public suwakOkno(suwak suwak)
        {
            // TODO: Complete member initialization
            this.suwak = suwak;
            InitializeComponent();
            el.Max = suwak.Max;
            el.Opis = suwak.Opis;
            el.Jednostka = suwak.Jednostka;
            el.przelicznik = suwak.przelicznik;
            el.Value = suwak.Value;
            el.splecione = suwak;
            suwak.splecione = el;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            suwak.splecione = null;
            el.splecione = null;
        }
    }
}
