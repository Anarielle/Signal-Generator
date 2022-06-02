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
        readonly double[] dataY = new double[10000];
        readonly Dictionary<double, double> dataXY = new Dictionary<double, double>();
        readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        readonly ScottPlot.Plottable.VLine VerticalLine;
        int NextIndex = 0;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer dispatcherTimer1 = new DispatcherTimer();

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


            pOriginalSignal.Plot.SetAxisLimits(0, 250, -1, 1);
            pOriginalSignal.Plot.SetOuterViewLimits(0, 10000);
            pOriginalSignal.Plot.AddSignal(dataY);
            VerticalLine = pOriginalSignal.Plot.AddVerticalLine(0, width: 2);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Tick += timer1Tick;
            dispatcherTimer.Start();

            dispatcherTimer1.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer1.Tick += timer2Tick;
            dispatcherTimer1.Start();

            dgDots.ItemsSource = dataXY;
        }

        public void AddDataPoint()
        {
            double amplitude = sAmplitude.Value;
            double phase = sPhase.Value * Math.PI / 180;
            double frequency = sFrequency.Value;
            double time = frequency * Stopwatch.Elapsed.TotalSeconds;

            if ((bool)rbHarmonic.IsChecked)
            {
                dataY[NextIndex] = amplitude * Math.Sin(2 * Math.PI * time + phase);
                dataXY.Add(Math.Round(Stopwatch.Elapsed.TotalSeconds, 3), Math.Round(dataY[NextIndex], 3));
            }
            if ((bool)rbSquare.IsChecked)
            {
                dataY[NextIndex] = amplitude * Math.Sign(Math.Sin(2f * Math.PI * time + phase));
                dataXY.Add(Math.Round(Stopwatch.Elapsed.TotalSeconds, 3), Math.Round(dataY[NextIndex], 3));
            }
            if ((bool)rbTriangle.IsChecked)
            {
                dataY[NextIndex] = amplitude * (1f - 4f * (float)Math.Abs(Math.Round(time + phase - 0.25f) - (time + phase - 0.25f)));
                dataXY.Add(Math.Round(Stopwatch.Elapsed.TotalSeconds, 3), Math.Round(dataY[NextIndex], 3));
            }

            dgDots.Items.Refresh();
            NextIndex++;
            if (NextIndex >= this.dataY.Length)
                NextIndex = 0;

            var xLimits = pOriginalSignal.Plot.GetAxisLimits();
            if (xLimits.XMax < NextIndex)
            {
                pOriginalSignal.Plot.SetAxisLimitsX(NextIndex, NextIndex + 250);
            }

            VerticalLine.X = NextIndex;
        }

        private void timer1Tick(object sender, EventArgs e)
        {
            AddDataPoint();
        }

        private void timer2Tick(object sender, EventArgs e)
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
        int count = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            count++;
            if (count % 2 == 1)
            {
                dispatcherTimer.Stop();
                dispatcherTimer1.Stop();
                bStop.Content = "Продолжить";
            }

            if (count % 2 == 0)
            {
                dispatcherTimer.Start();
                dispatcherTimer1.Start();
                bStop.Content = "Остановить";
            }
        }
    }
}
