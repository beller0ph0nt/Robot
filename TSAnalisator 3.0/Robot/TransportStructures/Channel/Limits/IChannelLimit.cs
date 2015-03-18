
namespace Robot.TransportStructures
{
    public interface IChannelLimit
    {
        double K { get; set; }
        double B { get; set; }

        double F(double x);
        void CalcLimit(IPoint p1, IPoint p2);
        void CalcOffset(IPoint p);
        string ToString();
    }
}
