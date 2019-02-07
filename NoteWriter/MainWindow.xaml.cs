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
        System.Windows.Threading.DispatcherTimer mainTimer;


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
            float frec = SoundCalculator.GetFrequency(e.pickData);
            lbFrec.Content = frec;
            lbNote.Content = SoundCalculator.NoteFromFrequency(frec).ToString();
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
            renderer.Render(capturer.LastSound, 0);
        }

        private void LoadSoundData()
        {
            StreamReader sr = new StreamReader(@"..\..\data\sounds.txt");
            float[] buf = new float[49];

            for (int i = 0; i < 49; i++)
            {
                buf[i] = (float)int.Parse(sr.ReadLine());
            }

            SoundCalculator.SoundData = buf;
            
        }

    }
}
