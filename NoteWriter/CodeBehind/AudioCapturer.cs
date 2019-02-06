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
        private bool isRecording;
        private int laudPeak;
        private float lastPeakValue;


        public AudioCapturer()
        {
            m_waveSource = new NAudio.Wave.WaveIn();
            m_waveSource.WaveFormat = new NAudio.Wave.WaveFormat(44100, 16, 1);

            m_data = new List<float>();
            m_waveSource.DataAvailable += waveSourceDataAvailable;

            isRecording = false;
            laudPeak = 0;
            lastPeakValue = 0;
        }

        public List<float> Data { get => m_data; }

        public NAudio.Wave.WaveFormat WaveFormat { get => m_waveSource.WaveFormat; }
        public bool IsRecording { get => isRecording; }
        public float TimeInMemory { get => m_data.Count / 44.1f; }
        public int LaudPeak { get => laudPeak;}

        public void StartRecording()
        {
            m_waveSource.StartRecording();
            isRecording = true;
        }

        public void StopRecording()
        {
            m_waveSource.StopRecording();
            isRecording = false;
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

            laudPeak = 0;

            float[] avarageTab = new float[e.BytesRecorded / 2200 + 1];
            int x = 0;
            int j = (m_data.Count - e.BytesRecorded / 2);

            while ( j < m_data.Count-1100)
            {
                avarageTab[x] = SoundCalculator.GetAvarage(m_data, j, j + 1100);
                x++;
                j += 1100;
            }

            avarageTab[x] = SoundCalculator.GetAvarage(m_data, j, m_data.Count);

            int maxi = -1;

            x = 0;
            if (avarageTab[0] > lastPeakValue + 0.04f)
                maxi = 0;

            x = 1;
            while(x < avarageTab.Length)
            {
                if (avarageTab[x] > avarageTab[x-1] + 0.04f)
                    maxi = x;
                x++;
            }

            lastPeakValue = avarageTab[x-1];

            if (maxi != -1)
                laudPeak = m_data.Count - e.BytesRecorded / 2 + 1100 * maxi;
           // System.Windows.MessageBox.Show(m_waveSource.WaveFormat.SampleRate.ToString());
           // System.IO.File.WriteAllBytes("C:/Users/Piotr/Desktop/test.bin", e.Buffer);
        }
    }
}
