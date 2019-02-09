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
        
        static private NotesFinder notesFinder;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            LoadSoundData();

            var samples = getSamplesFromNoteFinder();

            Network net = new Network(new int[] { 268, 60, 60, 61 }, @"..\..\data\neuralNet.fnn");

            while(true)
            {
                net.TrainAsync(1, samples);
                net.Predict(samples[0]);
            }
            
        }

        static private void LoadSoundData()
        {
            try
            {
                Stream stream = File.Open(@"..\..\data\soundData.dat", FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
                    new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                notesFinder = (NotesFinder)bf.Deserialize(stream);
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
            foreach (var item in notesFinder.NoteModels.Keys)
            {
                double[] pred = new double[61];
                for (int i = 0; i < 61; i++)
                {
                    pred[i] = 0;
                    if (i == item.ToInt())
                        pred[i] = 1;
                }

                foreach (var d in notesFinder.NoteModels[item])
                {
                    double[] data = new double[d.Data.Count];
                    int i = 0;
                    foreach (var x in d.Data.Values)
                    {
                        data[i] = x;
                        i++;
                    }

                    res.Add(new Sample(data, pred));

                }
            }

            return res;
        }
    }
}