﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Window
    {

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.oknoLogowanie = null;
        }
        public Logowanie()
            : base()
        {

            InitializeComponent();
            UC = UserCon;
        }
        public LogowanieUC UC;
    }
}
