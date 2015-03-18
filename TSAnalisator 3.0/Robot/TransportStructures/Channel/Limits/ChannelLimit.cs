using System;

namespace Robot.TransportStructures
{
    public class ChannelLimit : IChannelLimit
    {
        public double K { get; set; }
        public double B { get; set; }

        public ChannelLimit(double k, double b)
        {
            K = k;
            B = b;
        }

        public double F(double x) { return (K * x + B); }

        public void CalcLimit(IPoint p1, IPoint p2)
        {
            try
            {
                K = (p2.Y - p1.Y) / (p2.X - p1.X);
                CalcOffset(p1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CalcOffset(IPoint p) { B = p.Y - K * p.X; }

        public override string ToString()
        {
            return string.Format("[k = {0} b = {1}]", K, B);
        }
    }
}
