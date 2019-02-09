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

        private static int FindFirst0(List<float> data)
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                if (data[i] * data[i + 1] < 0)
                    return i;
            }
            return -1;
        }

        private static int HowMany0(List<float> data)
        {
            int res = 0;
            for (int i = 0; i < data.Count - 1; i++)
            {
                if (data[i] * data[i + 1] < 0)
                    res++;
            }
            return res;
        }

        public static FrequencyModel GetFrequencyModel(List<float> data)
        {

            Dictionary<float, float> rawData = new Dictionary<float, float>();

            float maxV = data.Max();
            int adder = 4;

            int j = FindFirst0(data);
            if (j == -1)
                j = 0;

            float frec = 100;
            float d_frec = 2f;

            while (frec < 4300)
            {
                if (frec == 200 || frec == 400 || frec == 800 || frec == 1600 || frec == 3200)
                    d_frec *= 2;

                float sum = 0f;
                for (int i = j; i < data.Count; i += adder)
                {
                    sum += (float)Math.Abs(data[i] - maxV * Math.Sin((2 * Math.PI * frec / 44100) * (i - j)));
                }

                rawData.Add(frec, sum);
                frec += d_frec;
            }

            return new FrequencyModel(rawData);

        }

    }
}
