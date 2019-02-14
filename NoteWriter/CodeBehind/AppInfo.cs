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
    class AppInfo
    {
        private Label m_freq;
        private Label m_note;
        private Label m_active;
        private Label m_sensitivity;

        public AppInfo(Label fr, Label n, Label a, Label s)
        {
            m_freq = fr;
            m_note = n;
            m_active = a;
            m_sensitivity = s;
        }

        public void SetFrequency(float freq)
        {
            m_freq.Content = freq.ToString() + "Hz";
        }

        public void SetNote(Note n)
        {
            m_note.Content = n.ToString();
        }

        public void SetState(bool s)
        {
            if (s)
            {
                m_active.Content = "Capturing Sound";
                m_active.Foreground = new SolidColorBrush(Color.FromRgb(0, 0xBB, 0));
            }
        
            else
            {
                m_active.Content = "Not Capturing";
                m_active.Foreground = new SolidColorBrush(Color.FromRgb(0xBB, 0, 0));
            }
        }

        public void SetSensitivity(double s)
        {
            m_sensitivity.Content = s.ToString("0.000");
        }
    }
}
