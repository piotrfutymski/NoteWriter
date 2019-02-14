using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NeuralNet;
using System.Windows.Threading;



namespace NoteWriter
{
    /// <summary>
    /// Logika interakcji dla klasy ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        AudioCapturer capturer;
        List<Sample> samples;
        int samplesOneType;

        Note testedNote;

        DispatcherTimer timer;

        bool learningProcess = false;
        float lPr = 0;
        

        public ConfigWindow()
        {
            InitializeComponent();

            capturer = new AudioCapturer();
            capturer.NewPick += Capturer_NewPick;
            capturer.Dence = 0.0f;

            samples = new List<Sample>();

            samplesOneType = 0;

            testedNote = new Note(0);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,0,1);
            timer.Tick += Timer_Tick;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(learningProcess)
            {
                prgBar.Value = lPr;
            }
            else if(timer.Interval == new TimeSpan(0, 0, 1))
            {
                testedNote++;
                lbNote.Content = "Pause";
                if (testedNote.ToInt() == 0)
                {
                    learningProcess = true;
                    capturer.StopRecording();
                    btnStart.IsEnabled = false;
                    btnStop.IsEnabled = false;
                    lbNote.Content = "End";
                    prgBar.Value = 0;
                    Task.Factory.StartNew(() => Learn());
                }

                timer.Interval = new TimeSpan(0, 0, 0,0,10);
                prgBar.Value = testedNote.ToInt() * 100 / 61;
                samplesOneType = 0;
                lbSampleCount.Content = samplesOneType;
            }
            else
            {
                timer.Interval = new TimeSpan(0, 0, 1);
                lbNote.Content = testedNote.ToString();
            }
           

        }

        private void Learn()
        {
            Network net = new Network(new int[] { 268, 61 }, @"..\..\data\net.fnn");
            int i = 0;
            while (i < 300)
            {
                net.TrainAsync(2, samples);
                i++;
                float testValue = testNetwork(net);
                if (testValue > 0.95)
                    break;
               lPr = Math.Max(testValue * 100, i / 3);

            }

            this.Close();
        }

        private float testNetwork(Network net)
        {
            Random rnd = new Random();
            float res = 0f;
            for (int i = 0; i < 100; i++)
            {
                int s = rnd.Next(samples.Count);
                if (net.Predict(samples[s]) == nmSample(samples[s]))
                    res+=1;
            }
            return res / 100;
        }

        private int nmSample(Sample s)
        {
            for (int i = 0; i < s.Predictions.Length; i++)
            {
                if (s.Predictions[i] == 1)
                    return i;
            }
            return -1;
        }

        private void Capturer_NewPick(object sender, AudioPickEventArgs e)
        {
            var test = SoundCalculator.GetFrequencyModel(e.pickData);
            samples.Add(SoundCalculator.GetSampleFromModel(test, testedNote));
            samplesOneType++;
            lbSampleCount.Content = samplesOneType;
            lbAllSampleCount.Content = samples.Count;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnStop.IsEnabled = true;
                btnStart.IsEnabled = false;

                capturer.StartRecording();
                timer.Start();
            }           
            catch (NAudio.MmException ex)
            {
                System.Windows.MessageBox.Show(string.Format("{0} \n Try to connect a microphone.", ex.Message));
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            capturer.StopRecording();
            timer.Stop();
        }
    }
}
