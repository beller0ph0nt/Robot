
namespace Robot.TransportStructures
{
    public class DoubleMaxTrandAlgorithm : IChannelLimitsCalcAlgorithm
    {
        public void CalcLimits(IChannelLimit lowerLimit, IChannelLimit upperLimit, IBlock firstBlock, IBlock secondBlock)
        {
            upperLimit.CalcLimit(new Point(firstBlock.High.X, firstBlock.High.Y), new Point(secondBlock.High.X, secondBlock.High.Y));
            lowerLimit.K = upperLimit.K;
            lowerLimit.CalcOffset(new Point(firstBlock.Low.X, firstBlock.Low.Y));
        }
    }
}
