﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    [Serializable()]
    public class Note
    {
        public enum NTone
        {
            C, Cis, D, Dis, E, F, Fis, G, Gis, A, B, H
        }

        NTone tone;
        int height;


        public NTone Tone { get => tone; set => tone = value; }
        public int Height { get => height; set => height = value; }

        public override string ToString()
        {
            return (tone.ToString() + height.ToString());
        }

        public int ToInt()
        {
            return height * 12 + (int)tone - 36;
        }



    }
}
