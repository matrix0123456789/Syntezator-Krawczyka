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
        public event Action Wybrano;



        public UtwórSerwerUI(UtwórSerwer utwórSerwer, string login, string tytul)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            Tag = this.utwórSerwer = utwórSerwer;
            Autor.Content = login;
            Tytuł.Content = tytul;
            pubCheck.IsChecked = utwórSerwer.upr == UtwórSerwer.uprawnienia.publiczny;
        }

        private void Usuń_click(object sender, RoutedEventArgs e)
        {
            Statyczne.serwer.usuń(utwórSerwer);
            Statyczne.serwer.pobierzUtwory();


        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (utwórSerwer.upr == UtwórSerwer.uprawnienia.prywatny)
                Statyczne.serwer.ustaw_publiczne(utwórSerwer);
            else
                Statyczne.serwer.ustaw_prywatne(utwórSerwer);
            Statyczne.serwer.pobierzUtwory();
        }


        internal void WybranoDz()
        {
            if (Wybrano != null)
                Wybrano();
        }
    }
}
