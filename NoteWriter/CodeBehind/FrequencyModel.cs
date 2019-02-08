using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NoteWriter
{
    class FrequencyModel
    {
        private const float m_sensitivity = 0.8f;

        private Dictionary<float, float> m_Data;
        private float m_firstTone;
        private List<float> m_Tones; 
        private float m_avarage;

        public FrequencyModel(Dictionary<float, float>  rD)
        {
            List<float> largestValues = new List<float>();
            m_Data = new Dictionary<float, float>();
            m_Tones = new List<float>();
            m_avarage = rD.Values.Sum() / rD.Count;
            foreach (var item in rD)
            {
                m_Data.Add(item.Key, Math.Abs((item.Value - m_avarage)*10/m_avarage));
                if (m_Data[item.Key] > m_sensitivity)
                    largestValues.Add(item.Key);
            }
            m_firstTone = rD.Aggregate((l, r) => l.Value> r.Value ? l : r).Key;
            largestValues.Sort((l, r) => m_Data[l] > m_Data[r] ? 1 : -1);
            largestValues.Reverse();

            int i = 0;
            while (largestValues.Count > 0)
            {
                if (!IsInRadius(largestValues[i]))
                    m_Tones.Add(largestValues[i]);
                largestValues.Remove(largestValues[i]);
            }
        }

        private bool IsInRadius(float x)
        {
            foreach (var item in m_Tones)
            {
                if (Math.Abs(x - item) / x < 0.1)
                    return true;
            }
            return false;
        }

        public float FirstTone { get => m_firstTone; }
        public List<float> Tones { get => m_Tones; }

        public void SaveToFile(string filename)
        {
            StreamWriter sr = new StreamWriter(filename);
            foreach (var item in m_Data)
            {
                sr.WriteLine("{0};{1}", item.Key, item.Value);
            }
            sr.Close();
        }

        public string GetTonesInfo()
        {
            string res = "";
            foreach (var item in m_Tones)
            {
                res += String.Format("{0} - {1} ;", item.ToString(), (m_Data[item]/m_Data[m_Tones[0]]).ToString("0.00"));
            }
            return res;
        }

    }
}
