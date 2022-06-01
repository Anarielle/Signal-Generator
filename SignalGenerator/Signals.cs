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
        public double Amplitude { get; set; } = 0;
        public double Phase { get; set; } = 0;
        public double Frequency { get; set; } = 0;
        
        //public Signal(double amplitude, double phase, double frequency)
        //{
        //    Amplitude = amplitude;
        //    Phase = phase;
        //    Frequency = frequency;
        //}



        public double[] HarmonicSignal(double t)
        {
            double[] yValues = new double[500];
            for (int i = 0; i < yValues.Length; i++)
            {
                yValues[i] = Amplitude * (float)Math.Sin(2f * Math.PI * (Frequency * i + Phase));
            }

            return yValues;
        }

        public double[] TriangleSignal(double[] dataX)
        {
            double[] yValues = new double[500];
            for (int i = 0; i < yValues.Length; i++)
            {
                yValues[i] = Amplitude * (1f - 4f * (float)Math.Abs(Math.Round((Frequency * i + Phase) - 0.25f) - ((Frequency * i + Phase) - 0.25f)));
            }
            
            return yValues;

            //value = amplitude * (1f - 4f * (float)Math.Abs(Math.Round(t - 0.25f) - (t - 0.25f)));
        }

        public double[] SquareSignal()
        {           
            double[] yValues = new double[500];
            double pi = 0;
            for (int i = 0; i < yValues.Length; i++)
            {
                yValues[i] = Amplitude * Math.Sign(Math.Sin(2 * Math.PI * Frequency + Phase)); ;
                pi += Math.PI;
            }

            return yValues;

            //value = amplitude * Math.Sign(Math.Sin(2f * Math.PI * t));
        }
    }
}
