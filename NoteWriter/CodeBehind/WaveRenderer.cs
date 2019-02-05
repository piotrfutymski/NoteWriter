using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    class WaveRenderer
    {
        int m_height;
        int m_width;

        Bitmap m_rendered = null;

        public WaveRenderer() { }

        public Bitmap RenderedBitmap { get => m_rendered; }

        public void Init( int h, int w)
        {
            m_height = h;
            m_width = w;
        }

        public void Render(List<float> data, int offset)
        {
            m_rendered = new Bitmap(m_width, m_height);
            var g = Graphics.FromImage(m_rendered);
            var br = new SolidBrush(Color.Black);

            g.FillRectangle(br, 0, 0, m_width, m_height);

            Point pointA = new Point(0,0);
            Point pointB = new Point(0,0);

            var pen = new Pen(Color.Red);

            for (int i = offset; i < offset+m_width && i < data.Count; i++)
            {

                pointA.Y = (int)((m_height / 2) * (data[i] + 1));
                pointB.Y = pointA.Y + 1;

                g.DrawLine(pen, pointA, pointB);

                pointA.X += 1;
                pointB.X += 1;
            }

        }

       
    }
}
