using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNet;

namespace NoteWriter
{
    [Serializable()]
    public class NotesFinder
    {


        Dictionary<Note, List<FrequencyModel>> m_NoteModels;

        public NotesFinder()
        {
            m_NoteModels = new Dictionary<Note, List<FrequencyModel>>();
        }

        public Dictionary<Note, List<FrequencyModel>> NoteModels { get => m_NoteModels; set => m_NoteModels = value; }
    }
}
