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
    class Wstążka : Grid
    {
        chil ch;

        public UIElementCollection Children { get { return ch; } }
        public Wstążka()
        {
            ch = new chil(this);
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
            if (PodSpodem != null)
            {
                if (PodSpodem.Margin == null)
                    PodSpodem.Margin = new Thickness(0, ActualHeight, 0, 0);
                else
                    PodSpodem.Margin = new Thickness(PodSpodem.Margin.Left, ch.gora.ActualHeight + ch.dol.ActualHeight, PodSpodem.Margin.Right, PodSpodem.Margin.Bottom);
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
            foreach (var x in karty)
            {
                x.Key.Content = x.Value.ToString();
            }
            poprawPrzyciski();

        }
        static Brush Tło = new SolidColorBrush(Color.FromRgb(235, 253, 230));
        Wstążka parent;
        public WrapPanel gora = new WrapPanel();
        public Grid dol = new Grid();
        public chil(Wstążka a)
            : base(a, a)
        {
            parent = a;
            gora.Background = Brushes.White;
            gora.VerticalAlignment = VerticalAlignment.Top;
            // gora.MinHeight = 25;

            (parent as Grid).Children.Add(gora);
            dol.Background = Tło;
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
        Dictionary<Button, UIElement> karty = new Dictionary<Button, UIElement>();
        void but_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).Tag as UIElement).Visibility == Visibility.Visible)
            {
                ((sender as Button).Tag as UIElement).Visibility = Visibility.Collapsed;
                
            }
            else
            {
                foreach (var x in dol.Children)
                {
                    if (x == (sender as Button).Tag)
                        (x as UIElement).Visibility = Visibility.Visible;
                    else
                        (x as UIElement).Visibility = Visibility.Collapsed;
                }} poprawPrzyciski();
                if (parent.PodSpodem != null)
                {
                    var tah = ((sender as Button).Tag as FrameworkElement).ActualHeight;
                    if (tah < ((sender as Button).Tag as FrameworkElement).Height)
                        tah = ((sender as Button).Tag as FrameworkElement).Height;
                    if (((sender as Button).Tag as FrameworkElement).Visibility != Visibility.Visible)
                        tah = 0;
                    if (parent.PodSpodem.Margin == null)
                        parent.PodSpodem.Margin = new Thickness(0, gora.ActualHeight + tah, 0, 0);
                    else
                        parent.PodSpodem.Margin = new Thickness(parent.PodSpodem.Margin.Left, gora.ActualHeight + tah, parent.PodSpodem.Margin.Left, parent.PodSpodem.Margin.Bottom);
                }
            
        }

        private void poprawPrzyciski()
        {
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
    enum PrzyciskRozmiar { Duży, Średni, Mały }
    class Przycisk : Button
    {

        [Bindable(true)]
        public PrzyciskRozmiar Rozmiar
        {
            get { return _Rozmiar; }
            set
            {
                _Rozmiar = value;
                if (_Obrazek != null)
                {
                    przerysuj();
                }
            }
        }
        PrzyciskRozmiar _Rozmiar = PrzyciskRozmiar.Duży;
        static Thickness marginesyDuży = new Thickness(2);
        void przerysuj()
        {
            switch (_Rozmiar)
            {
                case PrzyciskRozmiar.Duży:
                    _Obrazek.Width = 32;
                    _Obrazek.Height = 32;
                    _Obrazek.Margin = marginesyDuży;
                    _Obrazek.HorizontalAlignment = HorizontalAlignment.Center;
                    _Obrazek.VerticalAlignment = VerticalAlignment.Top;
                    MinWidth = 40;
                    MinHeight = 60;
                   // wew.MinWidth = 36;
                   // wew.MinHeight = 58;
                    _podpis.HorizontalAlignment = HorizontalAlignment.Center;
                    _podpis.VerticalAlignment = VerticalAlignment.Top;
                    _podpis.Visibility = Visibility.Visible;
                    _podpis.TextWrapping = TextWrapping.Wrap;
                    _podpis.TextAlignment = TextAlignment.Center;
                    _podpis.Margin = new Thickness(0, 32, 0, 0);
                   // wew.HorizontalAlignment = HorizontalAlignment.Stretch;
                    // wew.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case PrzyciskRozmiar.Średni:
                    MinWidth = 80;
                   // wew.MinWidth = 74;
                    _Obrazek.Width = 16;
                    _Obrazek.Height = 16;
                    _Obrazek.HorizontalAlignment = HorizontalAlignment.Left;
                    _podpis.HorizontalAlignment = HorizontalAlignment.Left;
                    _podpis.VerticalAlignment = VerticalAlignment.Center;
                    _podpis.Visibility = Visibility.Visible;
                    _podpis.Margin = new Thickness(0, 0, 0, 0);
                    _podpis.TextWrapping = TextWrapping.NoWrap;
                    _podpis.TextAlignment = TextAlignment.Left;
                    Height = 22;
                    MinHeight = 22;
                    //wew.MinHeight = 18;
                    //wew.HorizontalAlignment = HorizontalAlignment.Left;
                    //wew.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case PrzyciskRozmiar.Mały:
                    MinWidth = 22;
                    MinHeight = 22;
                   // wew.MinHeight = 18;
                   // wew.MinWidth = 18;
                    _Obrazek.Width = 16;
                    _Obrazek.Height = 16;
                    _podpis.Visibility = Visibility.Collapsed;
                    Height = 22;
                    Width = 22;
                    break;
            }
        }

        [Bindable(true)]
        public string Podpis
        {
            get { return _Podpis; }
            set
            {
                try
                {
                    _podpis.Text = value;
                }
                catch { }
                _Podpis = value;
            }
        }
        public string _Podpis = "Podpis";

        [Bindable(true)]
        public Canvas Obrazek
        {
            get { return _Obrazek; }
            set
            {
                lock (Children)
                {
                    if (_Obrazek != null)
                        Children.Remove(_Obrazek);
                    Children.Insert(0,value);
                    _Obrazek = value;
                    przerysuj();
                }

            }
        }
        Canvas _Obrazek = null;
        TextBlock _podpis = new TextBlock();
        Grid wew = new Grid();
        public Przycisk()
        {
            this.VerticalContentAlignment = VerticalAlignment.Stretch;
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            Children = wew.Children;
            AddChild(wew);
            this.BorderThickness = new Thickness(0);
            //wew.Background = Brushes.Red;
            //Rozmiar = PrzyciskRozmiar.Duży;
            Background = Brushes.Azure.Clone();
            Background.Opacity = 0;
            
            _podpis.VerticalAlignment = VerticalAlignment.Bottom;
            Children.Add(_podpis);
            Rozmiar = PrzyciskRozmiar.Duży;
            Margin = new Thickness(0);
        }

        public UIElementCollection Children;
    }
}
