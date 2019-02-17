using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NoteWriter
{
    class Song
    {
        List<SongNote> m_songNotes;
        DateTime startTime;
        string title;

        SongRenderer m_renderer;

        public Song(SongRenderer r)
        {
            startTime = DateTime.Now;
            m_songNotes = new List<SongNote>();
            m_renderer = r;
            m_renderer.RenderBackground();
        }

        public string Title { get => title; set => title = value; }
        public DateTime StartTime { get => startTime;}

        public void AddNote(Note n)
        {
            TimeSpan s = DateTime.Now - startTime;
            m_songNotes.Add(new SongNote(s, n));
        }


        public TimeSpan GetLastNoteTime()
        {
            if (m_songNotes.Count == 0)
                return new TimeSpan(0);
            return (DateTime.Now - startTime - m_songNotes.Last().Time);
        }

        public void RenderNotes()
        {
            var l = new List<SongNote>();
            if(DateTime.Now - startTime < m_renderer.TimeLine)
            {
                foreach (var item in m_songNotes)
                {
                    l.Add(item);
                }
                m_renderer.RenderNotes(l, new TimeSpan(0, 0, 0));
            }
            else
            {
                foreach (var item in m_songNotes)
                {
                    if (item.Time > DateTime.Now - startTime - m_renderer.TimeLine)
                        l.Add(item);                      
                }
                m_renderer.RenderNotes(l,  DateTime.Now - startTime - m_renderer.TimeLine);
            }


        }
    }
}
