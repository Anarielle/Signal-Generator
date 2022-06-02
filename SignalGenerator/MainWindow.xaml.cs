using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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
            rbHarmonic.IsChecked = true;
        }

        public void AddDataPoint()
        {
            pHarmonics.Reset();
            pOriginalSignal.Reset();
            pSpectrum.Reset();
            
            pOriginalSignal.Plot.SetAxisLimits(0, 50, -1, 1);
            pOriginalSignal.Plot.SetOuterViewLimits(0, 10000);
            pHarmonics.Plot.SetAxisLimits(0, 50, -1 ,1);
            pHarmonics.Plot.SetOuterViewLimits(0, 10000);
            pSpectrum.Plot.SetAxisLimits(0, 20, -1, 1);
            pSpectrum.Plot.SetOuterViewLimits(0, 10000);

            var func1 = signal.BuildSignal();
            pOriginalSignal.Plot.AddFunction(func1);
            //double[] second = Generate.Sinusoidal(10000, 1, Frequency * 2, Amplitude / 2);
            //double[] third = Generate.Sinusoidal(10000, 1, Frequency * 4, Amplitude / 4);
            //pHarmonics.Plot.AddSignal(second);
            //pHarmonics.Plot.AddSignal(third);


            double period = 1 / signal.Frequency;
            var firstHarmonica = new Func<double, double?>((x) => signal.Amplitude / 3 * Math.Sin(2 * Math.PI * signal.Frequency * 3 * x + signal.Phase));
            var secondHarmonica = new Func<double, double?>((x) => signal.Amplitude / 5 * Math.Sin(2 * Math.PI * signal.Frequency * 5 * x + signal.Phase));

            pOriginalSignal.Plot.AddFunction(func1);
            pHarmonics.Plot.AddFunction(func1);
            pHarmonics.Plot.AddFunction(firstHarmonica);
            pHarmonics.Plot.AddFunction(secondHarmonica);

            Complex[] samples = new Complex[1000];
            for (int i = 0; i < 1000; i++)
            {
                samples[i] = new Complex((double)(func1.Invoke(i) + firstHarmonica.Invoke(i) + secondHarmonica.Invoke(i)), 0);
            }

            double[] times = new double[200];
            for (int i = 0; i < 200; i++)
            {
                times[i] = ((i + 1.0) / 1000) / 2;
            }

            Fourier.Forward(samples, FourierOptions.NoScaling);

            double[] mag = new double[100];
            double[] hzPerSeconds = new double[100];

            for (int i = 0; i < 100; i++)
            {
                mag[i] = (2.0 / 1000) * (Math.Abs(Math.Sqrt(Math.Pow(samples[i].Real, 2) + Math.Pow(samples[i].Imaginary, 2))));
                hzPerSeconds[i] = 1000 / 1000 * i;
            }

            pSpectrum.Plot.AddBar(hzPerSeconds, mag);

            pOriginalSignal.Refresh();
            pHarmonics.Refresh();
            pSpectrum.Refresh();

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
            signal.Amplitude = sAmplitude.Value;
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
