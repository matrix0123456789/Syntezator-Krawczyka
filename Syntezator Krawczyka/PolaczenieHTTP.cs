using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace Syntezator_Krawczyka
{
    enum stanSerwera { oczekiwanie, połączono, błąd }
    public class PolaczenieHTTP
    {
        public string login;
        int publiczny, laczony, suma, wysylaj;
        int prywatny = (ushort)(new Random()).Next(16777215);
        string sesjaPHP;
        const string Base64Str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        public bool zalogowano = false;
        stanSerwera stan = stanSerwera.oczekiwanie;
        static PolaczenieHTTP()
        {

        }

        public PolaczenieHTTP()
        {
            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            {
                byte licz = 0;
            start:
                try
                {
                    WebClient polaczenie = new WebClient();
                    string kody = polaczenie.DownloadString("http://syntezator.aq.pl/json.php?kody");
                    var grupy = Regex.Match(polaczenie.ResponseHeaders["Set-cookie"], "PHPSESSID=([a-zA-Z0-9]*);").Groups;
                    sesjaPHP = grupy[1].Value;
                    //sesjaPHP = polaczenie.ResponseHeaders["Set-cookie"];
                    var poRegExpie = Regex.Match(kody, "\\{\"publiczny\":([0-9]+),\"laczony\":([0-9]+)\\}");
                    publiczny = int.Parse(poRegExpie.Groups[1].Value);
                    laczony = int.Parse(poRegExpie.Groups[2].Value);
                    wysylaj = (int)(((long)publiczny * (long)prywatny) % 16777216);
                    suma = (int)(((long)laczony * (long)prywatny) % 16777216);
                    stan = stanSerwera.połączono;

                }
                catch
                {
                    if (licz++ < 3)
                    {
                        Thread.Sleep(6000);
                        goto start;
                    }
                    else
                    {
                        stan = stanSerwera.błąd;
                        Thread.Sleep(60000);
                        goto start;
                    }
                }
            }, null);
        }
        public long id = -1;
        static Regex Regexlogowanie = new Regex("{\"status\":\"([^\"]*)\"(,\"id\":\"([0-9]+)\")?}");
        static Regex RegexUtwory = new Regex("\"utwory\":\\[(.*)\\]");
        internal void loguj(string login, string haslo)
        {
            this.login = login;
            this.haslo = haslo;
            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            {
                while (stan == stanSerwera.oczekiwanie)
                    Thread.Sleep(100);
                if (stan == stanSerwera.błąd)
                {
                    MessageBox.Show("Błąd", "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    try
                    {
                        WebClient polaczenie = new WebClient();
                        polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka");
                        polaczenie.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        var ret = polaczenie.UploadString("http://syntezator.aq.pl/json.php?phpsession=" + sesjaPHP, "POST", koduj("{\"login\":\"" + login + "\",\"haslo\":\"" + haslo + "\"}"));
                        if (Regexlogowanie.IsMatch(ret))
                        {
                            var odp = Regexlogowanie.Match(ret);
                            if (odp.Groups[1].Value == "ok")
                            {
                                MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                                {
                                    MainWindow.oknoLogowanie.Close();
                                    zalogowano = true;
                                    MainWindow.thi.zmianaLogowania(this);
                                });
                                id = long.Parse(odp.Groups[3].Value);
                                pobierzUtwory();
                            }
                            else { MessageBox.Show(odp.Groups[1].Value); }
                        }
                        else { MessageBox.Show(ret); }
                    }
                    catch
                    {
                        MessageBox.Show("Błąd", "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }, this);
        }

        private string koduj(string co)
        {
            // co = Convert.ToBase64String(co.ToCharArray());
            /* var bajty = new byte[co.Length];
             for (var i = 0; i < co.Length; i++)
             {
                 bajty[i] = (byte)co[i];

             }
             co = Convert.ToBase64String(bajty);
             var ret = "a=" + wysylaj + "&b=";
             for (var i = 0; i < co.Length; i++)
             {
                 int key;
                 if (i % 4 == 0)
                     key = (suma / 262144);
                 else if (i % 4 == 1)
                     key = (suma / 4096) % 64;
                 else if (i % 4 == 2)
                     key = (suma / 64) % 64;
                 else
                     key = suma % 64;
                 if (co[i] == '=')
                     ret += '=';
                 else
                     ret += Base64Str[Base64Str.IndexOf(co[i]) ^ key];
             }
             return ret;*/



            var bajty = new byte[co.Length];
            for (var i = 0; i < co.Length; i++)
            {
                bajty[i] = (byte)co[i];

            }
            co = Convert.ToBase64String(bajty);
            var ret = "b=" + co;
            return ret;
        }

        public string haslo { get; set; }

        public void pobierzUtwory()
        {
            // System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            // {

            Statyczne.serwer.utworyZalogowanego = pobierzUtwory(id.ToString());
            if (Statyczne.serwer.utworyZalogowanego == null)
                MessageBox.Show("Błąd", "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (MainWindow.oknoLogowanie != null)
                MainWindow.oknoLogowanie.wyswietlUtwory(Statyczne.serwer.utworyZalogowanego);
            // });
        }
        public UtworySerwer pobierzUtwory(string autor)
        {
            while (stan == stanSerwera.oczekiwanie)
                Thread.Sleep(100);
            if (stan == stanSerwera.błąd)
            {
                return null;
            }
            else
            {
                try
                {
                    WebClient polaczenie = new WebClient();
                    polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka");
                    polaczenie.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    var json = polaczenie.UploadString("http://syntezator.aq.pl/json.php?phpsession=" + sesjaPHP, "POST", koduj("{\"utwory\":\"" + id + "\"}"));
                    var ret = new UtworySerwer();
                    if(RegexUtwory.IsMatch(json))
                    {
                        var mat = RegexUtwory.Match(json);
                        var utwory = mat.Groups[1].Value.Split(',');
                        for (int i = 0; i < utwory.Length;i+=4 )
                            ret.Add(new UtwórSerwer(utwory[i] + ',' + utwory[i+1] + ',' + utwory[i+2] + ',' + utwory[i+3]));
                    }

                    MessageBox.Show(json);
                    return ret;
                }
                catch { return null; }
            }
        }

        internal UtworySerwer utworyZalogowanego;

        internal void wyślij(plik plik)
        {
            WebClient polaczenie = new WebClient();
            polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka");
            polaczenie.Headers.Add("Content-Type", "audio/x-syntezator-krawczyka");

            polaczenie.UploadData("http://syntezator.aq.pl/json.php?phpsession=" + sesjaPHP, plik.zapiszDoZmiennej());
                    
        }
    }

}
