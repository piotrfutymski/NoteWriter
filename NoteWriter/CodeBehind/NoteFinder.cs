using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteWriter
{
    class NoteFinder
    {

        NeuralNet.Network m_noteNetwork;


        public NoteFinder(string filename)
        {
            m_noteNetwork = new NeuralNet.Network(filename);
        }

        public Note getNoteFromModel(FrequencyModel model)
        {
           return  (Note)(m_noteNetwork.Predict(SoundCalculator.GetSampleFromModel(model)));
        }    

    }
}
