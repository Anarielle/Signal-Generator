using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace SignalGenerator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly double[] dataY = new double[500];
        readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        readonly ScottPlot.Plottable.VLine VerticalLine;
        int NextIndex = 0;

        public MainWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            pOriginalSignal.Plot.Title("Исходный сигнал");
            pRecievedSignal.Plot.Title("Полученный сигнал");
            pHarmonics.Plot.Title("Гармоники");
            pSpectrum.Plot.Title("Спектр исходного сигнала");
            sFrequency.IsSnapToTickEnabled = true;
            sFrequency.TickFrequency = 0.1;

            pOriginalSignal.Plot.SetAxisLimits(0, dataY.Length, -2, 2);
            pOriginalSignal.Plot.SetOuterViewLimits(0, 500);
            pOriginalSignal.Plot.AddSignal(dataY);
            VerticalLine = pOriginalSignal.Plot.AddVerticalLine(0, width: 2);

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Tick += timer1_Tick;
            dispatcherTimer.Start();

            DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer1.Tick += timer2_Tick;
            dispatcherTimer1.Start();
        }

        public Signal signal = new Signal();

        public void AddDataPoint()
        {
            double amplitude = sAmplitude.Value;
            double phase = sPhase.Value;
            double frequency = sFrequency.Value;
            double period = 1 / frequency;
            double time = frequency * Stopwatch.Elapsed.TotalSeconds;

            if ((bool)rbHarmonic.IsChecked)
            {
                this.dataY[NextIndex] = amplitude * Math.Sin(2 * Math.PI * time + phase);
            }
            if ((bool)rbSquare.IsChecked)
            {
                this.dataY[NextIndex] = amplitude * Math.Sign(Math.Sin(2f * Math.PI * time + phase)); 
            }
            if ((bool)rbTriangle.IsChecked)
            {                
                this.dataY[NextIndex] = amplitude * (1f - 4f * (float)Math.Abs(Math.Round(time + phase - 0.25f) - (time + phase - 0.25f)));
            }

            NextIndex += 1;
            if (NextIndex >= this.dataY.Length)
                NextIndex = 0;

            VerticalLine.X = NextIndex;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AddDataPoint();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pOriginalSignal.Render();
        }

        private void mHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Интерактивные элементы управления:" +
                $"\n     - перетаскивание левой кнопкой мыши: перемещение по графику" +
                $"\n     - перетаскивание правой кнопкой мыши: масштабирование" +
                $"\n     - перетаскивание средней кнопкой мыши: масштабирование области" +
                $"\n     - колесо прокрутки: масштабирование" +
                $"\n     - средний щелчок: подгонка данных" +
                $"\n     - щелчок правой кнопкой мыши: меню развертывания");
        }
    }
}
