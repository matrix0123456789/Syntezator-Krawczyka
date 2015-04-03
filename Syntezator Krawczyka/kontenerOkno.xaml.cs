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
    /// Interaction logic for kontenerOkno.xaml
    /// </summary>
    public partial class kontenerOkno : Window, IDisposable
    {
        public kontenerOkno(UIElement a)
        {
            InitializeComponent();
            kont.Children.Add(a);
            kont.ch.zmianaNazwy += ch_zmianaNazwy;
            Title = a.ToString();
        }

        void ch_zmianaNazwy(string obj)
        {
            Title = obj;
        }

        public void Dispose()
        {
            foreach (UIElement x in kont.ch.karty.Values)
            {
                if (kontenerOkien.gdzieJest.ContainsKey(x))
                    kontenerOkien.gdzieJest.Remove(x);
                try
                {
                    (x as IDisposable).Dispose();
                }
                catch { }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
