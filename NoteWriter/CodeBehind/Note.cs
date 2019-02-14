using System;
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

        public Note(int n)
        {
            Note t = (Note)(n);
            Tone = t.Tone;
            Height = t.Height;
        }

        public Note() { }

        public static Note operator++(Note n)
        {
            if (n.height != 8)
                return (Note)(n.ToInt() + 1);
            else
                return (Note)(0);
        }

        public static explicit operator Note(int n)
        {
            Note res = new Note();
            n += 36;
            res.Tone = (NTone)(n % 12);
            res.Height = n / 12 ;

            return res;
        }

        public int ToInt()
        {
            return height * 12 + (int)tone - 36;
        }



    }
}
