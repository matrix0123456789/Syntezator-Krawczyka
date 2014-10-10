using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.ComponentModel;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka
{
    class Wstążka : Grid
    {
        chil ch;

        public UIElementCollection Children { get { return ch; } }
        public Wstążka()
        {
            ch = new chil(this);
            //VerticalAlignment = VerticalAlignment.Top;
            SizeChanged += Wstążka_SizeChanged;
        }
       public void Wstążka_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PodSpodem != null)
            {
                if (PodSpodem.Margin == null)
                    PodSpodem.Margin = new Thickness(0, ActualHeight, 0, 0);
                else
                    PodSpodem.Margin = new Thickness(PodSpodem.Margin.Left, ch.gora.ActualHeight+ch.dol.ActualHeight, PodSpodem.Margin.Right, PodSpodem.Margin.Bottom);
            }
            ch.load();
        }
        FrameworkElement _PodSpodem = null;
        [Bindable(true)]
        [Category("Wstążka")]
        public FrameworkElement PodSpodem
        {
            set
            {
                if (_PodSpodem != null)
                    base.Children.Remove(_PodSpodem);
                value.VerticalAlignment = VerticalAlignment.Stretch;
                value.HorizontalAlignment = HorizontalAlignment.Stretch;
                //value.Margin = new Thickness(0, ActualHeight, 0, 0);
                base.Children.Add(value);
                _PodSpodem = value;
            }
            get { return _PodSpodem; }
        }

    }
    class WstążkaKarta : Grid
    {
        [Bindable(true)]
        [Category("Wstążka")]
        public string Tytuł { get; set; }
        public override string ToString()
        {
            return Tytuł;
        }
    }
    class chil : UIElementCollection
    {
        public void load()
        {
            foreach(var x in karty)
            {
                x.Key.Content = x.Value.ToString();
            }
        }
        Wstążka parent;
        public WrapPanel gora = new WrapPanel();
       public  Grid dol = new Grid();
        public chil(Wstążka a)
            : base(a, a)
        {
            parent = a;
            gora.Background = Brushes.Red;
            gora.VerticalAlignment = VerticalAlignment.Top;
            gora.MinHeight = 25;

            (parent as Grid).Children.Add(gora);
            dol.Background = Brushes.Blue;
            dol.VerticalAlignment = VerticalAlignment.Top;

            (parent as Grid).Children.Add(dol);
            gora.SizeChanged += gora_SizeChanged;
        }

        void gora_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dol.Margin = new Thickness(0, gora.ActualHeight, 0, 0);
        }
        public override int Add(UIElement element)
        {
            var but = new Button();
            but.Click += but_Click;
            but.Tag = element;
            but.VerticalAlignment = VerticalAlignment.Top;
            but.HorizontalAlignment = HorizontalAlignment.Left;
            gora.Children.Add(but);
            dol.Children.Add(element);
            karty.Add(but, element);
            //but.Content = element;
           
            return 10;

        }
        Dictionary<Button, UIElement> karty = new Dictionary<Button, UIElement>();
        void but_Click(object sender, RoutedEventArgs e)
        {
            foreach (var x in dol.Children)
            {
                if (x == (sender as Button).Tag)
                    (x as UIElement).Visibility = Visibility.Visible;
                else
                    (x as UIElement).Visibility = Visibility.Collapsed;
            }
            if (parent.PodSpodem != null)
            {
                var tah = ((sender as Button).Tag as FrameworkElement).ActualHeight;
                if (tah < ((sender as Button).Tag as FrameworkElement).Height)
                    tah = ((sender as Button).Tag as FrameworkElement).Height;
                if (parent.PodSpodem.Margin == null)
                    parent.PodSpodem.Margin = new Thickness(0, gora.ActualHeight + tah, 0, 0);
                else
                    parent.PodSpodem.Margin = new Thickness(parent.PodSpodem.Margin.Left, gora.ActualHeight + tah, parent.PodSpodem.Margin.Left, parent.PodSpodem.Margin.Bottom);
            }
        }
        public override void Remove(UIElement element)
        {
            foreach (var x in dol.Children)
            {
                if (x == element)
                    dol.Children.Remove(element);
            } foreach (var x in gora.Children)
            {
                if ((x as Button).Tag == element)
                    gora.Children.Remove(x as Button);
            }
        }
        public override void RemoveAt(int index)
        {
            dol.Children.RemoveAt(index);
            gora.Children.RemoveAt(index);
        }
        public override void RemoveRange(int index, int count)
        {
            while (count > 0)
            {
                RemoveAt(index);
                count--;
            }
        }
        public override void Clear()
        {

            dol.Children.Clear();
            gora.Children.Clear();
        }
    }
}
