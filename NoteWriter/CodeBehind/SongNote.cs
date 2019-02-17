using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    class SongNote : Note
    {
        private TimeSpan time;

        public SongNote(TimeSpan t, Note n)
        {
            Time = t;
            Height = n.Height;
            Tone = n.Tone;
        }

        public TimeSpan Time { get => time; set => time = value; }
    }
}
