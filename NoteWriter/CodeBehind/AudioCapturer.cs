using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;

namespace NoteWriter
{
    public class AudioPickEventArgs : EventArgs
    {
        public readonly List<float> pickData;
        public AudioPickEventArgs(List<float> pD)
        {
            pickData = pD;
        }
    }

    class AudioCapturer
    {

        public delegate void NewPickHandler(object sender, AudioPickEventArgs e);

        public event NewPickHandler NewPick;


        private NAudio.Wave.WaveIn m_waveSource;
        private List<float> m_lastSample;
        private List<float> m_actualSample;
        private bool m_isRecording;
        private List<float> m_laudPeak;

        private int m_rate;
        private int m_sampleTime;
        private int m_sampleCount;

        private float m_Dence;


        public AudioCapturer(int sT = 25, int r = 44100)
        {
            m_rate = r;
            m_sampleTime = sT;
            m_sampleCount = (sT * r / 1000);
            m_Dence = 0.04f;

            m_waveSource = new NAudio.Wave.WaveIn();
            m_waveSource.WaveFormat = new NAudio.Wave.WaveFormat(r, 16, 1);

            m_lastSample = new List<float>();
            m_actualSample = new List<float>();
            m_laudPeak = new List<float>();

            m_waveSource.DataAvailable += waveSourceDataAvailable;

            m_isRecording = false;
        }


        public NAudio.Wave.WaveFormat WaveFormat { get => m_waveSource.WaveFormat; }
        public bool IsRecording { get => m_isRecording; }
        public List<float> LastSound { get => m_lastSample;}
        public float Dence { get => m_Dence; set => m_Dence = value; }

        public void StartRecording()
        {
            m_waveSource.StartRecording();
            m_isRecording = true;
        }

        public void StopRecording()
        {
            m_waveSource.StopRecording();
            m_isRecording = false;
        }


        private void waveSourceDataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if(m_waveSource != null)
            {
                int i = 0;
                while(i < e.BytesRecorded )
                {
                    while(m_actualSample.Count < m_sampleCount && i < e.BytesRecorded)
                    {
                        short s = (short)((e.Buffer[i + 1] << 8) | (e.Buffer[i]));
                        m_actualSample.Add(s / 32768f);
                        i += 2;
                    }
                    if(m_lastSample.Count == 0)
                    {
                        m_lastSample = new List<float>();
                        m_lastSample.AddRange(m_actualSample);
                    }
                    else if(m_lastSample.Count == m_sampleCount && m_actualSample.Count == m_sampleCount)
                    {
                        if (SoundCalculator.GetAvarage(m_actualSample) > m_Dence + SoundCalculator.GetAvarage(m_lastSample))
                            NewPick?.Invoke(this, new AudioPickEventArgs(m_actualSample));
                        m_laudPeak = new List<float>();
                        m_laudPeak.AddRange(m_actualSample);
                        m_lastSample = new List<float>();
                        m_lastSample.AddRange(m_actualSample);
                        m_actualSample = new List<float>();
                    }
                    
                }
            }

        }
    }
}
