using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Syntezator_Krawczyka
{
    public class test { bool a = true;}
    /// <summary>
    /// zawiera niektóre elementy, które są statyczne, a nie było dla nich lepszego miejsca
    /// </summary>
   public  class Statyczne
    {
       public static PolaczenieHTTP serwer;
        public static BufferedWaveProvider bufor=new BufferedWaveProvider(new WaveFormat((int)plik.Hz,2));
        public static WasapiOut WasapiWyjście = new WasapiOut(AudioClientShareMode.Shared, 100);
        public static String[] nazwyDźwięków = { "C","C♯","D","D♯","E","F","F♯","G","G♯","A","B","H"};
        public Statyczne()
        {
           serwer= new PolaczenieHTTP();
            WasapiWyjście.Init(bufor);
            WasapiWyjście.Play();
        }
        public static bool debugowanie = false;
        static public plik otwartyplik;
        /// <summary>
        /// Tworzy informacje o plikach *.synkra w rejestrze systemowym.
        /// </summary>
        /// <remarks>Wymaga uprawnień administratora.</remarks>
        public static void skojarzPliki()
        {

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra", "", "PlikSyntezatoraKrawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra", "Content Type", "audio/x-syntezator-krawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra", "application", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra\\OpenWithList");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\.synkra\\OpenWithList", "a", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra\\OpenWithList\\syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(".synkra");

            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra", "", "URL:PlikSyntezatoraKrawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra", "URL Protocol", "");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra", "application", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("synkra\\OpenWithList");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\synkra\\OpenWithList", "a", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("synkra\\OpenWithList\\syntezator krawczyka.exe");


            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.synkra\\OpenWithList", "a", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.synkra", "application", "syntezator krawczyka.exe");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe");
            //Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\DefaultIcon");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\shell\\open\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\applications\\syntezator krawczyka.exe\\shell\\open\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" \"%1\"");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\shell\\Graj\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\applications\\syntezator krawczyka.exe\\shell\\Graj\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" -p \"%1\"");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("applications\\syntezator krawczyka.exe\\SupportedTypes");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\applications\\syntezator krawczyka.exe\\SupportedTypes", ".synkra", "");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("MIME\\Database\\Content Type\\audio/x-syntezator-krawczyka");//{E436EB83-524F-11CE-9F53-0020AF0BA770}, {A82E50BA-8E92-41eb-9DF2-433F50EC2993}
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\MIME\\Database\\Content Type\\audio/x-syntezator-krawczyka", "Extension", ".synkra");//HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Wow6432Node\CLSID\{083863F1-70DE-11D0-BD40-00A0C911CE86}\Instance\{11A947C3-BABC-466E-A678-1FFEC95EB2F8} sprawdzić
            //HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations dodać
            //{cd3afa76-b84f-48f0-9393-7edc34128127} sprawdzić

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka", "", "Plik programu SYntezator Krawczyka");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka", "FriendlyTypeName", "Plik programu SYntezator Krawczyka");

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\open");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\open\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka\\open\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" \"%1\"");

            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\Graj");
            Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("PlikSyntezatoraKrawczyka\\Graj\\command");
            Microsoft.Win32.Registry.SetValue("HKEY_CLASSES_ROOT\\PlikSyntezatoraKrawczyka\\Graj\\command", "", "\"" + System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName + "\" \"%1\" -p");

            Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\syntezator krawczyka.exe");
            Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\App Paths\\syntezator krawczyka.exe", "", System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

        }
    }
   public enum ModułyEnum { Sekwencer,Oscylator, Granie, cutoff, flanger}
}
