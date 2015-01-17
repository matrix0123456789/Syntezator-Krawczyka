using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for FeedBack.xaml
    /// </summary>
    public partial class FeedBack : Window
    {
        public FeedBack()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            WebClient polaczenie = new WebClient();
            polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka"+Statyczne.wersja);
            polaczenie.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var json = polaczenie.UploadString("http://jaebe.za.pl/musicstudio/feedback.php", "POST", "&t="+txt.Text);
            Close();
        }
    }
}
