#if JS
using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Syntezator_Krawczyka.JS
{
    public class Context:JavascriptContext
    {
        public Context()
        {
            SetParameter("Program", new Program());
        }
    }
    class Program
    {
        public void alert(string t)
        {
            MessageBox.Show(t);
        }
    }
}
#endif