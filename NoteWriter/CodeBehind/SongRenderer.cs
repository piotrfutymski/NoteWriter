using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace NoteWriter
{
    class SongRenderer
    {
        BitmapImage note;
        BitmapImage sharp;
        BitmapImage background;

        TimeSpan timeLine;

        List<Image> noteSymbols;
        List<Line> lineSymbols;
        List<Image> sharpSymbols;

        Canvas m_screen;

        public SongRenderer(string filenameNote, string filenameSharp, string filenameBackground, Canvas screen)
        {
            note = new BitmapImage();
            note.BeginInit();
            note.UriSource = new Uri(System.IO.Path.GetFullPath(filenameNote));
            note.EndInit();

            sharp = new BitmapImage();
            sharp.BeginInit();
            sharp.UriSource = new Uri(System.IO.Path.GetFullPath(filenameSharp));
            sharp.EndInit();

            background = new BitmapImage();
            background.BeginInit();
            background.UriSource = new Uri(System.IO.Path.GetFullPath(filenameBackground));
            background.EndInit();

            TimeLine = new TimeSpan(0, 0, 10);

            noteSymbols = new List<Image>();
            lineSymbols = new List<Line>();
            sharpSymbols = new List<Image>();

            m_screen = screen;

            for (int i = 0; i < 500; i++)
            {
                Image imgN = new Image();
                imgN.Source = note;
                noteSymbols.Add(imgN);
                imgN.Visibility = Visibility.Hidden;
                screen.Children.Add(imgN);
                Canvas.SetZIndex(imgN, 0);

                Image imgS = new Image();
                imgS.Source = sharp;
                sharpSymbols.Add(imgS);
                imgS.Visibility = Visibility.Hidden;
                screen.Children.Add(imgS);
                Canvas.SetZIndex(imgS, 0);

                Line l = new Line();
                lineSymbols.Add(l);
                l.Visibility = Visibility.Hidden;
                l.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                l.StrokeThickness = 1;
                screen.Children.Add(l);
                Canvas.SetZIndex(l, 10);

            }

        }

        public TimeSpan TimeLine { get => timeLine; set => timeLine = value; }

        public void RenderBackground()
        {
            Image img = new Image();
            img.Source = background;

            m_screen.Children.Add(img);
            Canvas.SetZIndex(img, -10);
        }

        public void RenderNotes(List<SongNote> notes, TimeSpan t0)
        {
            for (int i = 0; i < 500; i++)
            {
                noteSymbols[i].Visibility = Visibility.Hidden;
                lineSymbols[i].Visibility = Visibility.Hidden;
                sharpSymbols[i].Visibility = Visibility.Hidden;
            }

            int n = 0;
            int l = 0;
            int s = 0;

            foreach (var note in notes)
            {
                RenderOneNote(note, t0, ref n, ref l, ref s);
            }
            
        }

        private void RenderOneNote(SongNote note, TimeSpan t0, ref int n, ref int l, ref int s)
        {
            Point prctPos = new Point();
            prctPos.X = (note.Time - t0).TotalMilliseconds / TimeLine.TotalMilliseconds;

            int[] adder = new int[] { 0, 0, 1, 1, 2, 3, 3, 4, 4, 5, 5, 6 };
            bool[] sharp = new bool[] { false, true, false, true, false, false, true, false, true, false, true, false };

            int h = (note.Height - 3) * 7+ 5 + adder[(int)note.Tone];
            if (note.Height > 6)
                h -= 7;

            prctPos.Y = (double)h / 34.0;

            Canvas.SetLeft(noteSymbols[n], prctPos.X * m_screen.ActualWidth - 8);
            Canvas.SetBottom(noteSymbols[n], prctPos.Y * m_screen.ActualHeight - 8);
            noteSymbols[n].Visibility = Visibility.Visible;
            n++;

            if(sharp[(int)note.Tone])
            {
                Canvas.SetLeft(sharpSymbols[s], prctPos.X * m_screen.ActualWidth - 24);
                Canvas.SetBottom(sharpSymbols[s], prctPos.Y * m_screen.ActualHeight - 8);
                sharpSymbols[s].Visibility = Visibility.Visible;
                s++;
            }

            if(h % 2 == 0)
            {
                lineSymbols[l].X1 = prctPos.X * m_screen.ActualWidth - 12;
                lineSymbols[l].X2 = prctPos.X * m_screen.ActualWidth + 12;
                lineSymbols[l].Y1 = m_screen.ActualHeight - prctPos.Y * m_screen.ActualHeight;
                lineSymbols[l].Y2 = m_screen.ActualHeight - prctPos.Y * m_screen.ActualHeight;
                lineSymbols[l].Visibility = Visibility.Visible;
                l++;
            }
            
            if(h >= 25)
            {
                for (int i = 24; i < h; i+= 2)
                {
                    lineSymbols[l].X1 = prctPos.X * m_screen.ActualWidth - 12;
                    lineSymbols[l].X2 = prctPos.X * m_screen.ActualWidth + 12;
                    lineSymbols[l].Y1 = m_screen.ActualHeight - ((double)i / 34.0) * m_screen.ActualHeight;
                    lineSymbols[l].Y2 = m_screen.ActualHeight - ((double)i / 34.0) * m_screen.ActualHeight;
                    lineSymbols[l].Visibility = Visibility.Visible;
                    l++;
                }
            }

        }
    }
}
