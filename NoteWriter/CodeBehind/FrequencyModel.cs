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
        private float firstTone;

        public FrequencyModel(Dictionary<float, float>  rD)
        {
            m_rawData = rD;
            firstTone = rD.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        public float FirstTone { get => firstTone; }

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
