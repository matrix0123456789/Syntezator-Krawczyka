using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Syntezator_Krawczyka.Synteza;
using System.Windows;

namespace Syntezator_Krawczyka
{
    interface wejście
    {
        UIElement UI{get;set;}
        soundStart sekw {get;set;}
        void działaj();
    }
}
