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
        public Instrument()
        {
            InitializeComponent();
            Children=wewnętrzny.Children;
        }
        public UIElementCollection Children;
    }
}
