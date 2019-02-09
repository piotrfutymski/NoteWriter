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

            float maxi = fitValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;

            var keys = new List<Note>(fitValue.Keys);

            foreach (var k in keys)
            {
                fitValue[k] = fitValue[k] / maxi;
            }

           res.Add(fitValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);
           return res;
        }

        private float FitnessValue(FrequencyModel sample, Note n)
        {

            float res = 0;

            for (int i = 0; i < m_NoteModels[n].Tones.Count; i++)
            {
                for (int j = 0; j < sample.Tones.Count; j++)
                {
                    float x = m_NoteModels[n].Tones[i] - sample.Tones[j];
                    res += (float)Math.Exp(-(x * x) / 400)* m_NoteModels[n].Data[m_NoteModels[n].Tones[i]] * sample.Data[sample.Tones[j]];
                }
            }

            return res;
        }

        internal Dictionary<Note, FrequencyModel> NoteModels { get => m_NoteModels; set => m_NoteModels = value; }
    }
}
