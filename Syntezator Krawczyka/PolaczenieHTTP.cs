using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Syntezator_Krawczyka
{
    public class PolaczenieHTTP
    {
        private string login;
        long wysylaj = 0;
        bool gotowe = false;
        public PolaczenieHTTP()
        {
            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            {

                WebClient polaczenie = new WebClient();
                string kody=polaczenie.DownloadString("http://syntezator.aq.pl/json.php");
            }, null);
        }


        internal void loguj(string login, string haslo)
        {
            this.login = login;
            this.haslo=haslo;
            System.Threading.ThreadPool.QueueUserWorkItem((Action) =>
            {
                while (!gotowe)
                    Thread.Sleep(100);
                WebClient polaczenie = new WebClient();
             //   polaczenie.BaseAddress = "http://syntezator.aq.pl/json.php";

                // Add a user agent header in case the 
                // requested URI contains a query.

                polaczenie.Headers.Add("user-agent", "SyntezatorKrawczyka");

                polaczenie.UploadString("http://syntezator.aq.pl/json.php", "POST", koduj("{\"login\":\""+login+"\",\"haslo\":\""+haslo+"\"}"));
            }, this);
        }

        private string koduj(string co)
        {
            //co = btoa(co);
	        var ret="a="+wysylaj+"&b=";
	        /*for(var i=0;i<co.length;i++)
	        {
		        if(i%4==0)
			        var key=Math.floor(suma/262144)
		        else if(i%4==1)
			        var key=Math.floor(suma/4096)%64
		        else if(i%4==2)
			        var key=Math.floor(suma/64)%64
		        else
			        var key=suma%64
			        if(co[i]=='=')
			        ret+='=';
			        else
			        ret+=Base64._keyStr[Base64._keyStr.indexOf(co[i])^key]
	        }*/
	        return ret;
        }

        public string haslo { get; set; }
    }
}
