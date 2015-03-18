
namespace Robot.TransportStructures
{
    public class ConsolidationAlgorithm : IChannelLimitsCalcAlgorithm
    {
        public void CalcLimits(IChannelLimit lowerLimit, IChannelLimit upperLimit, IBlock firstBlock, IBlock secondBlock)
        {
            lowerLimit.K = 0;
            lowerLimit.B = firstBlock.Low.Y;
            upperLimit.K = 0;
            upperLimit.B = firstBlock.High.Y;
        }
    }
}
