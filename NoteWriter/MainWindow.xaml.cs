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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Windows.Threading;

namespace NoteWriter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioCapturer capturer;
        WaveRenderer renderer;
        AppInfo appInfo;
        NoteFinder noteFinder;
        DispatcherTimer updateTimer;

        Song testSong;
        DateTime lastSound;

        public MainWindow()
        {
            InitializeComponent();

            capturer = new AudioCapturer();
            capturer.NewPick += Capturer_NewPick;
            slSens.DataContext = capturer;

            renderer = new WaveRenderer(cnvWave);

            appInfo = new AppInfo(lbFreq, lbNote, lbState,lbSens);

            noteFinder = new NoteFinder(@"..\..\data\net.fnn");

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            updateTimer.Tick += UpdateTimer_Tick;
            

            testSong = new Song(new SongRenderer(@"..\..\data\img\note.png", @"..\..\data\img\sharp.png", @"..\..\data\img\background.png", cnvSong));
            lastSound = DateTime.Now;

            
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            testSong.RenderNotes();
        }

        private void Capturer_NewPick(object sender, AudioPickEventArgs e)
        {
            if(DateTime.Now - lastSound > new TimeSpan(0,0,0,0,150))
            {
                var t0 = DateTime.Now;
                FrequencyModel test = new FrequencyModel(new Dictionary<float, float>());
                Task t = Task.Factory.StartNew(() => { test = SoundCalculator.GetFrequencyModel(e.pickData); });
                renderer.Render(e.pickData, 0);
                t.Wait();
                appInfo.SetFrequency(test.FirstTone);
                Note n = noteFinder.getNoteFromModel(test);
                appInfo.SetNote(n);

                testSong.AddNote(n, t0); 
            }
            lastSound = DateTime.Now;              

        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            capturer.StopRecording();
            btnPlay.IsEnabled = true;
            btnPause.IsEnabled = false;
            appInfo.SetState(false);
            updateTimer.Stop();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                capturer.StartRecording();
                btnPlay.IsEnabled = false;
                btnPause.IsEnabled = true;
                appInfo.SetState(true);
                updateTimer.Start();
            }
            catch (NAudio.MmException ex)
            {
                System.Windows.MessageBox.Show(string.Format("{0} \n Try to connect a microphone.", ex.Message));
            }
        }

        private void SlSens_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            appInfo.SetSensitivity(e.NewValue);
        }

        private void ItConfigInstr_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow cnfwindow = new ConfigWindow();
            cnfwindow.Show();

            cnfwindow.Closed += (s, ee) => { this.IsEnabled = true; noteFinder = new NoteFinder(@"..\..\data\net.fnn"); };

            this.IsEnabled = false;
        }


    }
}
