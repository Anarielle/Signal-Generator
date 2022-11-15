using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalGenerator
{
    public class Signal
    {
        public double Amplitude { get; set; }
        public double Phase { get; set; }
        public double Frequency { get; set; }
        public SignalType SignalType { get; set; }

        public Signal(double amplitude, double phase, double frequency, SignalType signalType)
        {
            Amplitude = amplitude;
            Phase = phase;
            Frequency = frequency;
            SignalType = signalType;
            BuildSignal();
        }

        public Func<double, double?> BuildSignal()
        {
            Func<double, double?> func1;
            switch (SignalType)
            {
                case SignalType.Harmonic:
                    func1 = new Func<double, double?>((x) => Amplitude * Math.Sin(2 * Math.PI * Frequency/10 * x + Phase));
                    break;
                case SignalType.Triangle:
                    func1 = new Func<double, double?>((x) => Amplitude * (1f - 4f * (float)Math.Abs(Math.Round(x * Frequency/10 + Phase - 0.25f) - (x * Frequency/10 + Phase - 0.25f))));
                    break;
                default:
                    func1 = new Func<double, double?>((x) => Amplitude * Math.Sign(Math.Sin(2f * Math.PI * x * Frequency/10 + Phase)));
                    break;
            }

            return func1;
        }
    }
}
