using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syntezator_Krawczyka
{
    public class SkładoweHarmoniczne:FalaNiestandardowa
    {
        public List<float> Składowe = new List<float>();
        public SkładoweHarmoniczne()
        {
            Składowe.Add(1);
        }
        public float[] generujJedenPrzebieg(long długość)
        {
            var ret=new float[długość];
            for(int i=0;i<Składowe.Count;i++)
            {
                var stała= Math.PI * 2 / długość*(i+1);
                var głośność=Składowe[i];
                for(int i2=0;i2<długość;i2++)
                {
                    ret[i2] += (float)Math.Sin(i2 * stała)*głośność;
                }
            }
            return ret;
        }
    }
}
