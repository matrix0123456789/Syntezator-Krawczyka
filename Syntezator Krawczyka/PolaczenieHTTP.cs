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
    public class PolaczenieHTTP
    {
        public string login;
        int publiczny, laczony, suma, wysylaj;
        int prywatny = (ushort)(new Random()).Next(16777215);
        string sesjaPHP;
        const string Base64Str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        public bool zalogowano = false;
        bool gotowe = false;
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
                    sesjaPHP = Regex.Match(polaczenie.ResponseHeaders["Set-cookie"], "PHPSESSID.*PHPSESSID=([a-zA-Z0-9]*);").Groups[1].Value;
                    //sesjaPHP = polaczenie.ResponseHeaders["Set-cookie"];
                    var poRegExpie = Regex.Match(kody, "\\{\"publiczny\":([0-9]+),\"laczony\":([0-9]+)\\}");
                    publiczny = int.Parse(poRegExpie.Groups[1].Value);
                    laczony = int.Parse(poRegExpie.Groups[2].Value);
                    wysylaj = (int)(((long)publiczny * (long)prywatny) % 16777216);
                    suma = (int)(((long)laczony * (long)prywatny) % 16777216);
                    gotowe = true;

                }
                catch (System.Net.WebException e)
                {
                    if (licz++ > 3)
                    {
                        Thread.Sleep(6000);
                        goto start;
                    }
                }
            }, null);
        }


        internal void loguj(string login, string haslo)
        {
            this.login = login;
            this.haslo = haslo;
            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            {
                while (!gotowe)
                    Thread.Sleep(100);
                WebClient polaczenie = new WebClient();
                //   polaczenie.BaseAddress = "http://syntezator.aq.pl/json2.php";

                // Add a user agent header in case the 
                // requested URI contains a query.

                polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka");
                polaczenie.Headers.Add("Cookie", "PHPSESSID=" + sesjaPHP);
                //polaczenie.Headers.Add("Cookie", sesjaPHP);
                polaczenie.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var ret = polaczenie.UploadString("http://syntezator.aq.pl/json.php", "POST", koduj("{\"login\":\"" + login + "\",\"haslo\":\"" + haslo + "\"}"));
                /*polaczenie.Headers.Add("User-Agent", "SyntezatorKrawczyka");
                polaczenie.Headers.Add("Cookie", "PHPSESSID=" + sesjaPHP);
                //polaczenie.Headers.Add("Cookie", sesjaPHP);
                polaczenie.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var ret2 = polaczenie.UploadString("http://syntezator.aq.pl/json2.php", "POST", koduj("{\"login\":\"" + login + "\",\"haslo\":\"" + haslo + "\"}"));
                ret2.ToString();*/
                if(ret=="{\"logowanie\":\"ok\"}")
                {
                    MainWindow.dispat.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, (ThreadStart)delegate()
                    {
                        MainWindow.oknoLogowanie.Close();
                        zalogowano = true;
                        MainWindow.thi.zmianaLogowania(this);
                    });
                }
                else { MessageBox.Show(ret); }
            }, this);
        }

        private string koduj(string co)
        {
           // co = Convert.ToBase64String(co.ToCharArray());
            var bajty=new byte[co.Length];
            for(var i=0;i<co.Length;i++)
            {
                bajty[i]=(byte)co[i];

            }
            co = Convert.ToBase64String(bajty);
            var ret = "a=" + wysylaj + "&b=";
            for(var i=0;i<co.Length;i++)
            {
                int key;
                if(i%4==0)
			 key=(suma/262144);
		else if(i%4==1)
			 key=(suma/4096)%64;
		else if(i%4==2)
			 key=(suma/64)%64;
		else
			 key=suma%64;
                if (co[i] == '=')
                    ret += '=';
                else
                    ret += Base64Str[Base64Str.IndexOf(co[i]) ^ key];
            }
            return ret;
        }

        public string haslo { get; set; }
    }
}
