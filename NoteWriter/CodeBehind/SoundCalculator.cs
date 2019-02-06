using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    static class SoundCalculator
    {
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


    }
}
