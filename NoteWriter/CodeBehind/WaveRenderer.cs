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
    class WaveRenderer
    {
        int m_height;
        int m_width;

        Line[] m_tLines;

        Canvas m_screen;

        public WaveRenderer(Canvas s)
        {
            m_screen = s;
            m_height = (int)s.Height;
            m_width = (int)s.Width;

            m_tLines = new  Line[m_width];

            for (int i = 0; i < m_width; i++)
            {
                m_tLines[i] = new Line();
            }

            foreach (var item in m_tLines)
            {
                m_screen.Children.Add(item);
                item.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        public void Render(List<float> data, int offset)
        {

            Point pointA = new Point(0,0);
            Point pointB = new Point(0,0);

            for (int i = offset; i < offset+m_width && i < data.Count; i++)
            {

                pointA.Y = (int)((m_height / 2) * (data[i] + 1));
                pointB.Y = pointA.Y + 1;

                m_tLines[pointA.X].X1 = pointA.X;
                m_tLines[pointA.X].X2 = pointB.X;
                m_tLines[pointA.X].Y1 = pointA.Y;
                m_tLines[pointA.X].Y2 = pointB.Y;

                m_tLines[pointA.X].StrokeThickness = 1;
                m_tLines[pointA.X].Stroke = System.Windows.Media.Brushes.Red;
                m_tLines[pointA.X].Visibility = System.Windows.Visibility.Visible;

                pointA.X += 1;
                pointB.X += 1;
            }

        }
       
    }
}
