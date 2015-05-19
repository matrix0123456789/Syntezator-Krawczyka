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
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MainWindow.thi.Window_KeyDown(sender, e);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            MainWindow.thi.Window_KeyUp(sender, e);

        }
        public void Grid_DragOver(object sender, DragEventArgs e)
        {
        }
        private void Grid_Drop(object sender, DragEventArgs e)
        {
           if (e.Data.GetData("jms/karta") != null)
            {

                var obj = e.Data.GetData("jms/karta");
                var objt = (Object[])obj;
                var elem = (FrameworkElement)objt[0];
                if (kont.Children.Contains(elem))
                    return;
                var staraWst = (kontenerOkienchil)objt[1];
                staraWst.Remove(elem);
                elem.Visibility = Visibility.Collapsed;
                kont.Children.Add(elem);
                e.Handled = true;

            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}
