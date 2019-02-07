﻿using System;
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

            int adder = 5;
            int frecAdder = 5;
            
            for(frec = 100; frec < 5000; frec+=frecAdder)
            {
                if (frec == 400 || frec==800 || frec == 1600 || frec == 3200)
                    frecAdder *= 2;

                float sumA = 0f;
                float sumB = 0f;
                for (int i = j; i < data.Count; i+=adder)
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

        public static float GetFrequency0(List<float> data)
        {
            return (44100 / (2 * data.Count / HowMany0(data)));
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



    }
}
