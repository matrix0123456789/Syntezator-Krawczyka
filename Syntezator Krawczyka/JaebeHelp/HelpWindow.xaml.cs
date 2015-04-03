using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Syntezator_Krawczyka.JaebeHelp
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private HelpDatabase helpDatabase;
        private string lang;

        Regex h1 = new Regex(@"^=.*=$");
        Regex h2 = new Regex(@"^==.*==$");
        Regex h3 = new Regex(@"^===.*===$");
        public HelpWindow(HelpDatabase helpDatabase, string lang)
        {
            InitializeComponent();
            this.helpDatabase = helpDatabase;
            this.lang = lang;
            rysuj();
        }
        public HelpWindow(HelpDatabase helpDatabase, string lang, string strona):this(helpDatabase,lang)
        {
            idz(strona);
        }
        void rysuj()
        {
            nazwy.Items.Clear();
            foreach (var el in helpDatabase.poNazwie[lang])
            {
                nazwy.Items.Add(el.Value.nazwa);
            }
        }

        private void nazwy_Selected(object sender, RoutedEventArgs e)
        {
            treść.Document.Blocks.Clear();
            if (nazwy.SelectedValue == null)
                return;
            idz(nazwy.SelectedValue as String);
        }
        void idz(string gdzie)
        {
            treść.Document.Blocks.Clear();
            try
            {
                var explode = helpDatabase.poNazwie[lang][gdzie].tre.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < explode.Length; i++)
                {
                    if (explode[i].Length == 0)
                        treść.Document.Blocks.Add(new Paragraph());
                    else if (h3.IsMatch(explode[i]))
                    {
                        Paragraph parag;
                        if ((treść.Document.Blocks.LastBlock as Paragraph).Inlines.Count == 0)
                        {
                            parag = (treść.Document.Blocks.LastBlock as Paragraph);
                        }
                        else
                            parag = new Paragraph();
                        parag.FontSize = 16;
                        treść.Document.Blocks.Add(parag);
                        append(explode[i].Substring(3, explode[i].Length - 6), treść);
                        treść.Document.Blocks.Add(new Paragraph());
                    }
                    else if (h2.IsMatch(explode[i]))
                    {
                        Paragraph parag;
                        if ((treść.Document.Blocks.LastBlock as Paragraph).Inlines.Count == 0)
                        {
                            parag = (treść.Document.Blocks.LastBlock as Paragraph);
                        }
                        else
                            parag = new Paragraph();
                        parag.FontSize = 20;
                        treść.Document.Blocks.Add(parag);
                        append(explode[i].Substring(2, explode[i].Length - 4), treść);
                        treść.Document.Blocks.Add(new Paragraph());
                    }
                    else if (h1.IsMatch(explode[i]))
                    {
                        Paragraph parag;
                        if ((treść.Document.Blocks.LastBlock as Paragraph).Inlines.Count == 0)
                        {
                            parag = (treść.Document.Blocks.LastBlock as Paragraph);
                        }
                        else
                            parag = new Paragraph();
                        parag.FontSize = 24;
                        treść.Document.Blocks.Add(parag);
                        append(explode[i].Substring(1, explode[i].Length - 2), treść);
                        treść.Document.Blocks.Add(new Paragraph());
                    }
                    else if (explode[i][0] == '*')
                    {

                        List parag;
                        /* if(treść.Document.Blocks.LastBlock.)
                         {}else*/
                        parag = new List();
                        // parag.FontSize = 24;
                        treść.Document.Blocks.Add(parag);
                        while (explode.Length > i && (explode[i][0] == '*'))
                        {
                            var paragraf = new Paragraph();
                            append(explode[i].Substring(1), paragraf);
                            var item = new ListItem(paragraf);
                            parag.ListItems.Add(item);
                            i++;
                        }
                        i--;
                        //append(explode[i].Substring(1), treść);
                        // treść.Document.Blocks.Add(new Paragraph());
                    }
                    else
                        append(explode[i], treść);
                }
            }
            catch (KeyNotFoundException) { }
        }
        void append(string txt, RichTextBox rtb)
        {
            if (rtb.Document.Blocks.LastBlock==null||rtb.Document.Blocks.LastBlock.GetType() != typeof(Paragraph))
                treść.Document.Blocks.Add(new Paragraph());
            append(txt, rtb.Document.Blocks.LastBlock as Paragraph);
        }
        void append(string txt, Paragraph rtb)
        {
            while (txt.IndexOf("[[", 0) != -1)
            {
                rtb.Inlines.Add(new Run(txt.Substring(0, txt.IndexOf("[[", 0))));
                txt = txt.Substring(txt.IndexOf("[[", 0) + 2);
                // var par=new Paragraph();
                var inl = new Hyperlink();
                inl.Foreground = Brushes.Blue;
                // Label label=new Label();
                //label.Content=txt.Substring(0, txt.IndexOf("]]", 0));
                var trlinku = txt.Substring(0, txt.IndexOf("]]", 0));
                if (trlinku.Contains('|'))
                {
                    inl.Inlines.Add(trlinku.Substring(trlinku.IndexOf('|') + 1));
                    inl.Tag = trlinku.Substring(0, trlinku.IndexOf('|'));
                }
                else
                {
                    inl.Inlines.Add(trlinku);
                    inl.Tag = trlinku;
                }
                inl.Click += inl_Click;
                inl.MouseLeftButtonDown += inl_Click;
                //if (rtb.Document.Blocks.LastBlock.GetType() == typeof(Paragraph))
                    rtb.Inlines.Add(inl);
                // par.Foreground = Brushes.Blue;
                // rtb.Document.Blocks.Add(par);
                // rtb.AppendText(txt.Substring(0, txt.IndexOf("]]", 0)));
                // rtb.Document.Blocks.Add(new Paragraph());
                txt = txt.Substring(2 + txt.IndexOf("]]", 0));
            }
            rtb.Inlines.Add(new Run(txt));
        }

        void inl_Click(object sender, RoutedEventArgs e)
        {
            nazwy.SelectedValue = ((sender as Hyperlink).Tag as String);
            idz(((sender as Hyperlink).Tag as String));
        }
    }
}