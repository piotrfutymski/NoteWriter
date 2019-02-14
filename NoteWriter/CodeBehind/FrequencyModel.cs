using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NoteWriter
{
    public class FrequencyModel
    {
        private float m_sensitivity = 0.8f;

        private Dictionary<float, float> m_Data;
        private float m_firstTone;
        private List<float> m_Tones; 

        public FrequencyModel(Dictionary<float, float>  rD, float s = 0.8f)
        {
            m_sensitivity = s;
            List<float> largestValues = new List<float>();
            m_Data = new Dictionary<float, float>();
            m_Tones = new List<float>();
            var avarage = rD.Values.Sum() / rD.Count;
            foreach (var item in rD)
            {
                m_Data.Add(item.Key, Math.Abs((item.Value - avarage)*10/avarage));
                if (m_Data[item.Key] > m_sensitivity)
                    largestValues.Add(item.Key);
            }

            GetTonesFromLargestValues(largestValues);
        }

        private void GetTonesFromLargestValues(List<float> largestValues)
        {
            largestValues.Sort((l, r) => m_Data[l] > m_Data[r] ? 1 : -1);
            largestValues.Reverse();
            if(largestValues.Count != 0)
                m_firstTone = largestValues[0];

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

        public Dictionary<float, float> GetHighestData()
        {
            Dictionary<float, float> res = new Dictionary<float, float>();
            foreach (var item in m_Data)
            {
                if (item.Value > m_sensitivity)
                    res.Add(item.Key, item.Value);
            }

            return res;
        }

        public float FirstTone { get => m_firstTone; }
        public List<float> Tones { get => m_Tones; }
        public Dictionary<float, float> Data { get => m_Data;}
        public float Sensitivity { get => m_sensitivity;}

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
                res += String.Format("{0} - {1} ;", item.ToString(), m_Data[item].ToString("0.00"));
            }
            return res;
        }

    }
}
