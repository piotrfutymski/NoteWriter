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
        private const float m_sensitivity = 0.1f;

        private Dictionary<float, float> m_rawData;
        private float m_firstTone;
        private float m_avarage;

        public FrequencyModel(Dictionary<float, float>  rD)
        {
            m_rawData = rD;
            m_avarage = rD.Values.Sum() / rD.Count;
            m_firstTone = rD.Aggregate((l, r) => Math.Abs(l.Value - m_avarage) > Math.Abs(r.Value - m_avarage) ? l : r).Key;
        }

        public float FirstTone { get => m_firstTone; }

        public void SaveToFile(string filename)
        {
            StreamWriter sr = new StreamWriter(filename);
            foreach (var item in m_rawData)
            {
                sr.WriteLine("{0};{1}", item.Key, item.Value);
            }
            sr.Close();
        }

    }
}
