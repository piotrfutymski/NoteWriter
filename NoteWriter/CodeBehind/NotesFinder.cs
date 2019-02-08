using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    class NotesFinder
    {
        Dictionary<Note, FrequencyModel> m_NoteModels;

        internal Dictionary<Note, FrequencyModel> NoteModels { get => m_NoteModels; set => m_NoteModels = value; }
    }
}
