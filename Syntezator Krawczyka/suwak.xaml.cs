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
    /// Interaction logic for suwak.xaml
    /// </summary>
    public partial class suwak : UserControl
    {
        public double Value
        {
            get
            {
                return główny.Value;
            }
            set
            {
                główny.Value.ToString();
            }
        }
        public double Maximum
        {
            get
            {
                return główny.Maximum;
            }
            set
            {
                główny.Value.ToString();
            }
        }
        string _ToolTip;
        public string ToolTip
        {
            get
            {
                return _ToolTip;
            }
            set
            {
                główny.Value.ToString();
            }
        }
        public suwak()
        {
            InitializeComponent();
        }
    }
}
