using System;

namespace Robot.TransportStructures
{
    public class EmptyLimitsCalcAlgorithm : IChannelLimitsCalcAlgorithm
    {
        public void CalcLimits(IChannelLimit lowerLimit, IChannelLimit upperLimit, IBlock firstBlock, IBlock secondBlock)
        {
            throw new Exception("Не определен алгоритм перерасчета границ канала.");
        }
    }
}
