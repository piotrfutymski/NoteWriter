using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNet;

namespace NoteWriter
{
    [Serializable()]
    public class NoteModels
    {


        Dictionary<Note, List<FrequencyModel>> m_NoteModels;

        public NoteModels()
        {
            m_NoteModels = new Dictionary<Note, List<FrequencyModel>>();
        }

        public Dictionary<Note, List<FrequencyModel>> NoteModelsData { get => m_NoteModels; set => m_NoteModels = value; }
    }
}
