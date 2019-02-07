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

            mainTimer = new System.Windows.Threading.DispatcherTimer();
            //mainTimer.Tick += MainTimer_Tick;
            mainTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            mainTimer.Start();

            slDence.DataContext = capturer;
        }

        private void Capturer_NewPick(object sender, AudioPickEventArgs e)
        {
            renderer.Render(e.pickData, 0);
            lbFrec.Content = SoundCalculator.GetFrequency(e.pickData);
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

      /*  private void RenderThreadFunc()
        {
            renderer.Render(capturer.Data, Math.Max(capturer.Data.Count - 900, 0));
            var bmp = renderer.RenderedBitmap;
            BitmapSource b = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero,
                System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            bmpWaves.Source = b;
        }*/

    }
}
