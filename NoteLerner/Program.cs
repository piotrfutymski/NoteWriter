using System;
using NoteWriter;
using NeuralNet;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Windows;


namespace NoteLerner
{
    class Program
    {
        static private NoteModels notesFinder;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Random rnd = new Random();
            LoadSoundData();

            var samples = getSamplesFromNoteFinder();

            Network net = new Network(new int[] { 268, 61 },@"..\..\data\neuralNet4.fnn");

            while(true)
            {
                int s = rnd.Next(samples.Count);
                net.TrainAsync(2, samples);
                Console.WriteLine("it hear {0}, should be {1}", net.Predict(samples[s]), nmSample(samples[s]));
            }
            
        }

        static int nmSample(Sample s)
        {
            for (int i = 0; i < s.Predictions.Length; i++)
            {
                if (s.Predictions[i] == 1)
                    return i;
            }
            return -1;
        }

        static private void LoadSoundData()
        {
            try
            {
                Stream stream = File.Open(@"..\..\data\soundData.dat", FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
                    new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                notesFinder = (NoteModels)bf.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                Console.WriteLine("error no soundData yet");
            }

        }

        static private List<Sample> getSamplesFromNoteFinder()
        {
            var res = new List<Sample>();
            foreach (var item in notesFinder.NoteModelsData.Keys)
            {
                double[] pred = new double[61];
                for (int i = 0; i < 61; i++)
                {
                    pred[i] = 0;
                    if (i == item.ToInt())
                        pred[i] = 1;
                }

                foreach (var d in notesFinder.NoteModelsData[item])
                {
                    double[] data = new double[d.Data.Count];
                    int i = 0;
                    foreach (var x in d.Data.Values)
                    {
                        if (x > 0.8f)
                            data[i] = x;
                        else
                            data[i] = 0;
                        i++;
                    }

                    res.Add(new Sample(data, pred));

                }
            }

            return res;
        }
    }
}