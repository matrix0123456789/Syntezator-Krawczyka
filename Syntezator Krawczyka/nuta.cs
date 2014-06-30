using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syntezator_Krawczyka.Synteza;

namespace Syntezator_Krawczyka
{
    /// <summary>
    /// Odpowiada pojedyńczej nucie, zawiera informacje i ewentualnie gotowy dźwięk
    /// </summary>
    public class nuta:ICloneable
    {
        /// <summary>
        /// długość jednego przebiegu fali
        /// </summary>
        public double ilepróbek;
        public bool czyPogłos = false;
        public double ilepróbekNaStarcie;
        /// <summary>
        /// Długość wciśnięcia klawisza (bez opadania dźwięku i pogłosu)
        /// </summary>
       public long długość;
        /// <summary>
        /// dźwięk
        /// </summary>
       public float[] dane;
        /// <summary>
        /// ile czasu po starcie melodii dana nuta ma być słyszana
        /// </summary>
       public long opuznienie = 0;
       static int ilenut = 0;
        public static int nowyid{get{return ilenut++;}}
       public long wygenerowanoWcześniej = 0;
       public System.Diagnostics.Stopwatch start;
        /// <summary>
        /// identyfikator
       /// </summary>
       public long id = 0;
       public long idOryginalne = 0;
        /// <summary>
        /// liczba losowa, urzywana np. przy flangerze
        /// </summary>
       public int los = (int)(losowanie.Next(100000) + 10000);
       static Random losowanie = new Random();
       public long generujOd = 0;
       public long generujDo = -1;
       /// <summary>
       /// przesunięcie rozpoczęnia grania w danych
       /// </summary>
       public long grajOd = 0;
       /// <summary>
       /// przesunięcie zakończenia grania w danych
       /// </summary>
       public long grajDo = 0;
       public float głośność = 1;
       //głośność w prawym i lewym kanale
       public float balans0 = 1;
       public float balans1 = 1;
       public byte kopiaInnaId = 0;///<summary>Zawiera liczbę jaka jest dodawana do id np. przy pogłosie</summary>
       public soundStart sekw;
       public Dictionary<long, gra> grająLokalne;
       public Boolean czyGotowe = false;
        //public static 
       public nuta()
       {
           idOryginalne=id = ilenut++;
           start = System.Diagnostics.Stopwatch.StartNew();
           
       }

       /// <summary>
       /// tworzy klon, ze zmienionym id, nie kopiuje danych dźwiękowych
       /// </summary>
       /// <returns>klon</returns>
       public Object Clone() 
       { 
              return MemberwiseClone();
       } 

       public nuta(double ilepróbek, long długość)
       {
           this.ilepróbek=ilepróbekNaStarcie = ilepróbek;
           this.długość = długość;
           idOryginalne=id = ilenut++;
           start = System.Diagnostics.Stopwatch.StartNew();
       }
       public nuta(double ilepróbek, long długość, long opuznienie)
       {
           this.ilepróbek = ilepróbekNaStarcie = ilepróbek;
           this.długość = długość;
           this.opuznienie = opuznienie;
           idOryginalne= id = ilenut++;
           start = System.Diagnostics.Stopwatch.StartNew();

       }

       internal object Clone(short oktawy)
       {
           object ret = MemberwiseClone();
           ((nuta)ret).ilepróbek = ((nuta)ret).ilepróbek / (Math.Pow(2, oktawy));
           return ret;
       }

       public string ToString()
       {
           return opuznienie.ToString();
       }
        public static int sortuj(nuta a,nuta b)
        {
            return (int)(a.opuznienie - b.opuznienie);
        }
    }
}
