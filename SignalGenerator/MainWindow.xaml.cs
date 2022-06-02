using MathNet.Numerics;
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
    public partial class MainWindow : System.Windows.Window
    {
        public double Amplitude { get; set; } = 1;
        public double Phase { get; set; } = 0;
        public double Frequency { get; set; } = 0.1;

        public Signal signal = new Signal(1,0,0.1,SignalType.Harmonic);
        
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
            pHarmonics.Plot.SetAxisLimitsX(0, 250);

            rbHarmonic.IsChecked = true;
        }

        public void AddDataPoint()
        {
            pHarmonics.Reset();
            pOriginalSignal.Reset();
            pOriginalSignal.Plot.AddFunction(signal.BuildSignal());
            double[] second = Generate.Sinusoidal(10000, 1, Frequency * 2, Amplitude / 2);
            double[] third = Generate.Sinusoidal(10000, 1, Frequency * 4, Amplitude / 4);
            pHarmonics.Plot.AddSignal(second);
            pHarmonics.Plot.AddSignal(third);
            pHarmonics.Refresh();
            pOriginalSignal.Refresh();

            //dgDots.Items.Refresh();
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

        private void mRealTime_Click(object sender, RoutedEventArgs e)
        {
            new RealTime().Show();
        }

        private void rbHarmonic_Checked(object sender, RoutedEventArgs e)
        {
            signal.SignalType = SignalType.Harmonic;
            AddDataPoint();
        }

        private void rbSquare_Checked(object sender, RoutedEventArgs e)
        {
            signal.SignalType = SignalType.Square;
            AddDataPoint();
        }

        private void rbTriangle_Checked(object sender, RoutedEventArgs e)
        {
            signal.SignalType = SignalType.Triangle;
            AddDataPoint();
        }

        private void sAmplitude_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            signal. Amplitude = sAmplitude.Value;
            AddDataPoint();
        }

        private void sFrequency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {            
            signal.Frequency = sFrequency.Value;
            AddDataPoint();
        }

        private void sPhase_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {           
            signal.Phase = sPhase.Value * Math.PI / 180;
            AddDataPoint();
        }
    }
}
