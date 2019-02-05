using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;

namespace NoteWriter
{
    class AudioCapturer
    {
        private List<float> m_data;
        private NAudio.Wave.WaveIn m_waveSource;

        public AudioCapturer()
        {
            m_waveSource = new NAudio.Wave.WaveIn();
            m_waveSource.WaveFormat = new NAudio.Wave.WaveFormat(44100, 16, 1);

            m_data = new List<float>();
            m_waveSource.DataAvailable += waveSourceDataAvailable;
        }

        public List<float> Data { get => m_data; }

        public NAudio.Wave.WaveFormat WaveFormat { get => m_waveSource.WaveFormat; }

        public void StartRecording()
        {
            m_waveSource.StartRecording();    
        }

        public void StopRecording()
        {
            m_waveSource.StopRecording();
        }

        private void waveSourceDataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if(m_waveSource != null && m_data != null)
            {
                for (int i = 0; i < e.BytesRecorded; i+= 2)
                {
                    short s = (short)((e.Buffer[i + 1] << 8) | (e.Buffer[i]));
                    m_data.Add( s / 32768f);
                }
            }

           // System.Windows.MessageBox.Show(m_waveSource.WaveFormat.SampleRate.ToString());
           // System.IO.File.WriteAllBytes("C:/Users/Piotr/Desktop/test.bin", e.Buffer);
        }
    }
}
