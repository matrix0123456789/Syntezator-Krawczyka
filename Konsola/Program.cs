

using System;
using System.Threading;
using Syntezator_Krawczyka;
using System.Security.Principal;
namespace Syntezator_Krawczyka.Konsola
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start!");

            string[] parametry = Environment.GetCommandLineArgs();
            bool otwarto = false;
            bool zamknij = false;
            var a=new test();
            Console.WriteLine(a.ToString());
            for (var x = 1; x < parametry.Length; x++)
            {

                Console.WriteLine(x);
                Console.WriteLine(parametry[x]);
                if (parametry[x] == "/d" || parametry[x] == "-d")
                {
                    Statyczne.debugowanie = true;
                   
                }
                else if (parametry[x] == "/z" || parametry[x] == "-z")
                {
                    zamknij = true;
                }
                else if (parametry[x] == "/s" || parametry[x] == "-s")
                {
                    //if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
                      //  Statyczne.skojarzPliki();
                }
                else if (!otwarto)
                {
                    //Statyczne.otwartyplik = new plik(parametry[1]);
                    otwarto = true;
                }
            }
            if (zamknij)
            {
                
            }
               
        }

    }
}
