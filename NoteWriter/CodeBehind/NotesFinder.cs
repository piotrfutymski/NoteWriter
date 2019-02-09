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

        internal Dictionary<Note, FrequencyModel> NoteModels { get => m_NoteModels; set => m_NoteModels = value; }
    }
}
