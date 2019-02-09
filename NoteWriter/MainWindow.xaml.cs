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

namespace NoteWriter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioCapturer capturer;
        WaveRenderer renderer;
        NotesFinder notesFinder;

        Note testedNote = null;
        List<FrequencyModel> notesBuffer;




        public MainWindow()
        {
            InitializeComponent();

            capturer = new AudioCapturer();
            capturer.NewPick += Capturer_NewPick;

            renderer = new WaveRenderer(cnvMain);

            notesFinder = new NotesFinder();
            notesBuffer = new List<FrequencyModel>();

            slDence.DataContext = capturer;

            LoadSoundData();
        }

        private void Capturer_NewPick(object sender, AudioPickEventArgs e)
        {
            renderer.Render(e.pickData, 0);
            var test = SoundCalculator.GetFrequencyModel(e.pickData);
            lbInfo.Content = test.GetTonesInfo();
            
            if(testedNote != null)
            {
                notesBuffer.Add(test);
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
            SaveSoundData();
        }

        private void LoadSoundData()
        {
            try
            {
                Stream stream = File.Open(@"..\..\data\soundData.dat", FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                notesFinder = (NotesFinder)bf.Deserialize(stream);
                stream.Close();
            }
            catch 
            {
                MessageBox.Show("No Sound Data Avaible");
            }
            
        }

        private void SaveSoundData()
        {
            try
            {
                Stream stream = File.Open(@"..\..\data\soundData.dat", FileMode.CreateNew);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bf.Serialize(stream, notesFinder);
                stream.Close();
            }
            catch (SerializationException e)
            {
                MessageBox.Show("Bug with saving Sound Data " + e.Message);
            }
        }

        private void btnTestPiano_Click(object sender, RoutedEventArgs e)
        {
            testedNote = new Note();
            Note.NTone t;
            Enum.TryParse(cbNoteTone.Text, out t);
            testedNote.Tone = t;

            int n;
            int.TryParse(cbNoteHeight.Text, out n);
            testedNote.Height = n;



        }

        private void btnStopTestPiano_Click(object sender, RoutedEventArgs e)
        {
            var colapsed = new FrequencyModel(notesBuffer);

            colapsed.SaveToFile(@"..\..\data\test.txt");

            notesFinder.NoteModels.Add(testedNote, colapsed);
            testedNote = null;
            notesBuffer.Clear();
        }
    }
}
