
namespace Robot.TransportStructures
{
    public class DoubleMinTrandAlgorithm : IChannelLimitsCalcAlgorithm
    {
        public void CalcLimits(IChannelLimit lowerLimit, IChannelLimit upperLimit, IBlock firstBlock, IBlock secondBlock)
        {
            lowerLimit.CalcLimit(new Point(firstBlock.Low.X, firstBlock.Low.Y), new Point(secondBlock.Low.X, secondBlock.Low.Y));
            upperLimit.K = lowerLimit.K;
            upperLimit.CalcOffset(new Point(firstBlock.High.X, firstBlock.High.Y));
        }
    }
}
