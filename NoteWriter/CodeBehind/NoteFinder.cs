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
            double[] data = new double[model.Data.Count];
            int i = 0;
            foreach (var x in model.Data.Values)
            {
                if (x > 0.8f)
                    data[i] = x;
                else
                    data[i] = 0;
                i++;
            }

            NeuralNet.Sample test = new NeuralNet.Sample(data, new double[61]);

           return  (Note)(m_noteNetwork.Predict(test));
        }

    }
}
