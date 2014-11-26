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
    /// Interaction logic for UtwórSerwerUI.xaml
    /// </summary>
    public partial class UtwórSerwerUI : UserControl
    {
        private UtwórSerwer utwórSerwer;
        private string login;
        private string tytul;

        

        public UtwórSerwerUI(UtwórSerwer utwórSerwer, string login, string tytul)
        {
            // TODO: Complete member initialization
            InitializeComponent();
           Tag= this.utwórSerwer = utwórSerwer;
            Autor.Content = login;
            Tytuł.Content = tytul;
        }
    }
}
