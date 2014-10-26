using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Syntezator_Krawczyka
{
    public enum Przelicznik : byte { Liniowo, Logarytmicznie, Kwadratowo }
    public class suwak : Slider
    {
        [Bindable(true)]
        [Category("Własne")]
        public string Opis
        {
            get
            {
                return _opis;
            }
            set
            {
                ToolTip = value + "\r\n" + Value + _jednostka;
                _opis = value;
            }
        }
        String _jednostka = "";
        public Przelicznik _przelicznik;
        [Bindable(true)]
        [Category("Własne")]
        public Przelicznik przelicznik
        {
            get { return _przelicznik; }
            set
            {
                _przelicznik = value;
                maksimumPrzelicz(_maks);

            }
        }

        [Bindable(true)]
        [Category("Własne")]
        public string Jednostka
        {
            get
            {
                return _jednostka;
            }
            set
            {
                ToolTip = _opis + "\r\n" + Value + value;
                _jednostka = value;
            }
        }
        string _opis;
        public suwak()
        {
            przelicznik = Przelicznik.Liniowo;
            ValueChanged += suwak_ValueChanged;
            MouseRightButtonDown += suwak_MouseRightButtonDown;
        }

        void suwak_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var okno = new suwakOkno();
            okno.Show();
        }

        void suwak_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var test = sender == this;
            ToolTip = _opis + "\r\n" + Value + Jednostka;


        }
        public double Value
        {
            get
            {
                switch (przelicznik)
                {
                    case Przelicznik.Liniowo:
                        return base.Value;
                    case Przelicznik.Kwadratowo:
                        return base.Value * base.Value;
                    case Przelicznik.Logarytmicznie:
                        return Math.Pow(10, base.Value);
                    default:
                        return base.Value;
                }
            }
            set
            {
                switch (przelicznik)
                {
                    case Przelicznik.Liniowo:
                        base.Value = value;
                        break;
                    case Przelicznik.Kwadratowo:
                        base.Value = Math.Sqrt(value);
                        break;
                    case Przelicznik.Logarytmicznie:
                        if (value == 0)

                            base.Value = value;
                        else
                            base.Value = Math.Log10(value);
                        break;
                    default:
                        base.Value = value;
                        break;
                }
            }
        }

        public double _maks;
        [Bindable(true)]
        [Category("Własne")]
        public double Max
        {
            get
            {
                switch (przelicznik)
                {
                    case Przelicznik.Liniowo:
                        return base.Maximum;
                    case Przelicznik.Kwadratowo:
                        return base.Maximum * base.Maximum;
                    case Przelicznik.Logarytmicznie:
                        return Math.Pow(10, base.Maximum);
                    default:
                        return base.Maximum;
                }
            }
            set
            {
                maksimumPrzelicz(value);
                _maks = value;
            }
        }
        void maksimumPrzelicz(double value)
        {
            switch (przelicznik)
            {
                case Przelicznik.Liniowo:
                    base.Maximum = value;
                    break;
                case Przelicznik.Kwadratowo:
                    base.Maximum = Math.Sqrt(value);
                    break;
                case Przelicznik.Logarytmicznie:
                    if (value == 0)

                        base.Maximum = value;
                    else
                        base.Maximum = Math.Log10(value);
                    break;
                default:
                    base.Maximum = value;
                    break;
            }
        }
        [Bindable(true)]
        [Category("Własne")]
        public double Minimum
        {
            get
            {
                switch (przelicznik)
                {
                    case Przelicznik.Liniowo:
                        return base.Minimum;
                    case Przelicznik.Kwadratowo:
                        return base.Minimum * base.Minimum;
                    case Przelicznik.Logarytmicznie:
                        return Math.Pow(10, base.Minimum);
                    default:
                        return base.Minimum;
                }
            }
            set
            {
                switch (przelicznik)
                {
                    case Przelicznik.Liniowo:
                        base.Minimum = value;
                        break;
                    case Przelicznik.Kwadratowo:
                        base.Minimum = Math.Sqrt(value);
                        break;
                    case Przelicznik.Logarytmicznie:
                        if (value == 0)

                            base.Minimum = value;
                        else
                            base.Minimum = Math.Log10(value);
                        break;
                    default:
                        base.Minimum = value;
                        break;
                }
            }
        }
    }
}
