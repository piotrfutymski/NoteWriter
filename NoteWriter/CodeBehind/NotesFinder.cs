using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    [Serializable()]
    class NotesFinder
    {
        Dictionary<Note, FrequencyModel> m_NoteModels;

        public NotesFinder()
        {
            m_NoteModels = new Dictionary<Note, FrequencyModel>();
        }

        public List<Note> GetPlayedNotes(FrequencyModel sample)
        {
            var res = new List<Note>();

            Dictionary<Note, float> fitValue = new Dictionary<Note, float>();
            foreach (var item in m_NoteModels.Keys)
            {
                fitValue.Add(item, FitnessValue(sample, item));
            }

           res.Add(fitValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);
           return res;
        }

        private float FitnessValue(FrequencyModel sample, Note n)
        {
            int i = 0;
            int j = 0;

            float res = 0;

            while(i < m_NoteModels[n].Tones.Count && j < sample.Tones.Count)
            {
                float x = m_NoteModels[n].Tones[i] - sample.Tones[j];
                res += (float)Math.Exp(-(x*x)/1000);

                if (i + 1 == m_NoteModels[n].Tones.Count)
                    j++;
                else if (j + 1 == sample.Tones.Count)
                    i++;
                else if (x < 0)
                    i++;
                else 
                    j++;
            }

            return res;
        }

        internal Dictionary<Note, FrequencyModel> NoteModels { get => m_NoteModels; set => m_NoteModels = value; }
    }
}
