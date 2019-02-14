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
           return  (Note)(m_noteNetwork.Predict(GetSampleFromModel(model)));
        }

        public NeuralNet.Sample GetSampleFromModel(FrequencyModel model)
        {
            double[] data = new double[model.Data.Count];
            int i = 0;
            foreach (var x in model.GetHighestData())
            {
                data[i] = x.Value;
                i++;
            }

            return new NeuralNet.Sample(data, new double[61]);
        }

        public NeuralNet.Sample GetSampleFromModel(FrequencyModel model, Note n)
        {
            
            double[] data = new double[model.Data.Count];
            int i = 0;
            foreach (var x in model.GetHighestData())
            {
                data[i] = x.Value;
                i++;
            }

            double[] pred = new double[61];
            pred[n.ToInt()] = 1;

            return new NeuralNet.Sample(data, pred);
        }

    }
}
