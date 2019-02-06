using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    static class SoundCalculator
    {

        public static float GetAvarage(List<float> data, int jmp = 1)
        {
            return GetAvarage(data, 0, data.Count, jmp);
        }

        public static float GetAvarage(List<float> data, int start, float stop, int jmp = 1)
        {
            float avarage = 0;
            for (int i = start; i < stop; i+=jmp)
            {
                avarage += Math.Abs(data[i]);
            }
            avarage = avarage* jmp / (stop-start);

            return avarage;
        }

        public static float GetFrequency(List<float> data)
        {
            float lastSum = 10000000f;
            float res = 0f;
            float maxV = data.Max();

            float frec = 100;
            int j = FindFirst0(data);
            if (j == -1)
                return res;
            
            for(frec = 100; frec < 5000; frec+=5f)
            {
                float sumA = 0f;
                float sumB = 0f;
                for (int i = j; i < data.Count; i+=1)
                {
                    sumA += (float)Math.Abs(data[i] - maxV * Math.Sin((2 * Math.PI * frec / 44100) * (i - j)));
                    sumB += (float)Math.Abs(data[i] - maxV * Math.Sin(Math.PI+(2 * Math.PI * frec / 44100) * (i - j)));
                }
                if (Math.Min(sumA, sumB) < lastSum)
                {
                    res = frec;
                    lastSum = Math.Min(sumA, sumB);
                }
                    
            }
            return res;
        }

        private static int FindFirst0(List<float> data)
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                if (data[i] * data[i + 1] < 0)
                    return i;
            }
            return -1;
        }

 

    }
}
