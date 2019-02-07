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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace NoteWriter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioCapturer capturer;
        WaveRenderer renderer;

        bool saveNext = false;
        string filename = @"..\..\data\testttttt0.txt";
        int n = 1;


        public MainWindow()
        {
            InitializeComponent();

            capturer = new AudioCapturer();
            capturer.NewPick += Capturer_NewPick;

            renderer = new WaveRenderer(cnvMain);

            slDence.DataContext = capturer;

            LoadSoundData();
        }

        private void Capturer_NewPick(object sender, AudioPickEventArgs e)
        {
            renderer.Render(e.pickData, 0);

            if(saveNext)
            {
                var test = SoundCalculator.GetFrequencyModel(e.pickData);
                test.SaveToFile(filename);
                saveNext = false;

                filename = filename.Substring(0, filename.Length - 6)+n.ToString()+".txt";
                n++;
            }
            
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                capturer.StartRecording();
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = true;
            }
            catch (NAudio.MmException ex)
            {
                System.Windows.MessageBox.Show(string.Format("{0} \n Try to connect a microphone.", ex.Message));
            }
           
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            capturer.StopRecording();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            saveNext = true;
        }

        private void LoadSoundData()
        {
            StreamReader sr = new StreamReader(@"..\..\data\sounds.txt");
            float[] buf = new float[61];

            for (int i = 0; i < 61; i++)
            {
                buf[i] = int.Parse(sr.ReadLine());
            }

            SoundCalculator.SoundData = buf;

            sr.Close();
        }

    }
}
