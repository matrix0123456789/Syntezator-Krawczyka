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

                    if (Syntezator_Krawczyka.Properties.Settings.Default.Login != "" && Syntezator_Krawczyka.Properties.Settings.Default.Haslo != "")
                        loguj(Syntezator_Krawczyka.Properties.Settings.Default.Login, Syntezator_Krawczyka.Properties.Settings.Default.Haslo);

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
                                Syntezator_Krawczyka.Properties.Settings.Default.Login = login;
                                Syntezator_Krawczyka.Properties.Settings.Default.Haslo = haslo;
                                Syntezator_Krawczyka.Properties.Settings.Default.Save();
                                MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                                {
                                    if (MainWindow.oknoLogowanie != null)
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
                    if (RegexUtwory.IsMatch(json))
                    {
                        var mat = RegexUtwory.Match(json);
                        var utwory = mat.Groups[1].Value.Split(',');
                        if (utwory.Length >= 4)
                            for (int i = 0; i < utwory.Length; i += 4)
                                ret.Add(new UtwórSerwer(utwory[i] + ',' + utwory[i + 1] + ',' + utwory[i + 2] + ',' + utwory[i + 3]));
                    }

                    // MessageBox.Show(json);
                    return ret;
                }
                catch { return null; }
            }
        }

        internal UtworySerwer utworyZalogowanego;

        internal void wyślij(plik plik)
        {
            var dane = new Dictionary<string, object>();
            dane.Add("pliki[]", plik.zapiszDoZmiennej());
            var ret = Submit("http://syntezator.aq.pl/json.php?phpsession=" + sesjaPHP, dane);

            /* byte[] bu = new byte[1000];
             ret.GetResponseStream().Read(bu, 0, 1000);
             MessageBox.Show(System.Text.Encoding.UTF8.GetString(bu, 0, 1000));*/
            /* WebClient polaczenie = new WebClient();
            // polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka");
            // polaczenie.Headers.Add("Content-Type", "audio/x-syntezator-krawczyka");

             polaczenie.UploadData("http://syntezator.aq.pl/json.php?phpsession=" + sesjaPHP, plik.zapiszDoZmiennej());







             var bytes = plik.zapiszDoZmiennej();
             var boundary = "-----------------------------99614912995";
             var boundaryBytes=Encoding.UTF8.GetBytes(  boundary );
             var dane = "Content-Disposition: form-data; name=\"pliki[]\"\r\n\r\n";
             var dane2 = System.Text.Encoding.UTF8.GetBytes(dane);
             HttpWebRequest request = HttpWebRequest.Create("http://syntezator.aq.pl/json.php?phpsession=" + sesjaPHP) as HttpWebRequest;
             request.ContentType = "multipart/form-data, boundary=" + boundary;
             request.UserAgent = "SyntezatorKrawczyka";
             request.Method = "POST";
             request.ContentLength = bytes.Length + dane.Length + 2 * boundaryBytes.Length;
             request.Timeout = (int)Math.Round((double)bytes.Length / 5120.0 * 1000.0, 0);
             if (request.Timeout < 8000) request.Timeout = 8000;
             using (Stream rStream = request.GetRequestStream())
             {
                 rStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                 rStream.Write(dane2, 0, dane2.Length);
                 rStream.Write(bytes, 0, bytes.Length);
                 rStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                 rStream.Close();
             }
             byte[] bu = new byte[1000];
             request.GetResponse().GetResponseStream().Read(bu, 0, 1000);
             MessageBox.Show( System.Text.Encoding.UTF8.GetString(bu, 0, 1000));*/
        }
        internal static HttpWebResponse Submit(string url, Dictionary<string, object> values)
        {
            Stream requestStream = new MemoryStream();
            string boundary = "---------------------846346834";
            foreach (var value in values)
            {
                byte[] boundWrite = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                requestStream.Write(boundWrite, 0, boundWrite.Length);
                if (value.Value is byte[])
                {
                    byte[] write = Encoding.UTF8.GetBytes(
                        "Content-Disposition: form-data; name=\"" + value.Key + "\"; filename=\"" + value.Key + "\"\r\n" +
                        "Content-Transfer-Encoding: binary\r\n" +
                        "Content-Type: application/octet-stream\r\n\r\n"
                    );
                    requestStream.Write(write, 0, write.Length);
                    requestStream.Write(value.Value as byte[], 0, (value.Value as byte[]).Length);
                }
                else
                {
                    byte[] write = Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + value.Key + "\"\r\n\r\n" + value.Value.ToString());
                    requestStream.Write(write, 0, write.Length);
                }
            }
            byte[] footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            requestStream.Write(footer, 0, footer.Length);
            // Dane do wysłania też powinny być ok. Postarałem się, aby były zgodne ze specyfikacją. Powinny ;]
            byte[] bytes = new byte[(int)requestStream.Length];
            requestStream.Position = 0;
            requestStream.Read(bytes, 0, bytes.Length);
            requestStream.Dispose();
            // Kopiowanie do tablicy bajtów działa znakomicie.
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "multipart/form-data, boundary=" + boundary;
            request.UserAgent = "Syntezator-krawczyka";
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.Timeout = (int)Math.Round((double)bytes.Length / 5120.0 * 1000.0, 0);
            if (request.Timeout < 8000) request.Timeout = 8000;
            using (Stream rStream = request.GetRequestStream())
            {
                rStream.Write(bytes, 0, bytes.Length);
                rStream.Close();
            }
            return request.GetResponse() as HttpWebResponse;
        }
    }

}
