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
using System.Windows.Controls.Primitives;

namespace Syntezator_Krawczyka
{
    class kontenerOkien : Grid
    {
        public kontenerOkienchil ch;
        public static Dictionary<UIElement, Window> gdzieJest = new Dictionary<UIElement, Window>();
        public UIElementCollection Children { get { return ch; } }
        [Bindable(true)]
        public bool ZawszePrzyciski { get; set; }
        public kontenerOkien()
        {
            ch = new kontenerOkienchil(this);
            //VerticalAlignment = VerticalAlignment.Top;
            SizeChanged += Wstążka_SizeChanged;
            this.Loaded += Wstążka_Loaded;
        }


        void Wstążka_Loaded(object sender, RoutedEventArgs e)
        {
            Wstążka_SizeChanged(null, null);
            // ch.
        }
        public void Wstążka_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ch.load();
        }

    }
    class kontenerOkienchil : UIElementCollection
    {
        public void load()
        {
            foreach (var x in karty)
            {
                x.Key.Content = x.Value.ToString();
            }
            poprawPrzyciski();

        }
        public event Action<String> zmianaNazwy;
        static Brush Tło = new SolidColorBrush(Color.FromRgb(235, 253, 230));
        kontenerOkien parent;
        public WrapPanel gora = new WrapPanel();
        public Grid dol = new Grid();
        public kontenerOkienchil(kontenerOkien a)
            : base(a, a)
        {
            parent = a;
            gora.Background = Brushes.White;
            gora.VerticalAlignment = VerticalAlignment.Top;
            // gora.MinHeight = 25;

            (parent as Grid).Children.Add(gora);
            dol.Background = Brushes.White;
            dol.VerticalAlignment = VerticalAlignment.Stretch;
            dol.HorizontalAlignment = HorizontalAlignment.Stretch;
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
            but.Padding = new Thickness(5, 2, 5, 2);
            but.Background = Brushes.White;
            but.Click += but_Click;
            but.Tag = element;
            but.Margin = new Thickness(0, 3, 0, 0);
            but.BorderThickness = new Thickness(0, 0, 1, 0);
            but.VerticalAlignment = VerticalAlignment.Top;
            but.HorizontalAlignment = HorizontalAlignment.Left;
            gora.Children.Add(but);
            dol.Children.Add(element);
            
            karty.Add(but, element);
            //but.Content = element;
            poprawPrzyciski();
            return 10;

        }
        public Dictionary<Button, UIElement> karty = new Dictionary<Button, UIElement>();
        void but_Click(object sender, RoutedEventArgs e)
        {
          /*  if (((sender as Button).Tag as UIElement).Visibility == Visibility.Visible)
            {
                ((sender as Button).Tag as UIElement).Visibility = Visibility.Collapsed;

            }
            else*/
            {
                podpis = (sender as Button).Tag.ToString();
                if (zmianaNazwy != null)
                    zmianaNazwy(podpis);
                foreach (var x in dol.Children)
                {
                    if (x == (sender as Button).Tag)
                        (x as UIElement).Visibility = Visibility.Visible;
                    else
                        (x as UIElement).Visibility = Visibility.Collapsed;
                }
            } poprawPrzyciski();

        }
        String podpis = "";
        public override string ToString()
        {
            return podpis;
        }
        private void poprawPrzyciski()
        {
            if (parent.ZawszePrzyciski || gora.Children.Count >1)
                gora.Visibility = Visibility.Visible;
            else
                gora.Visibility = Visibility.Collapsed;
            foreach (var x in gora.Children)
            {
                if (((x as Button).Tag as UIElement).Visibility == Visibility.Visible)
                    (x as Button).Background = Tło;
                else
                    (x as Button).Background = Brushes.White;
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
    public class zawartośćOkna:UserControl
    {
        [Bindable(true)]
        public string Title { get; set; }
        [Bindable(true)]
        public event EventHandler Closed;
        public override string ToString()
        {
            return Title;
        }
    }
   
}
