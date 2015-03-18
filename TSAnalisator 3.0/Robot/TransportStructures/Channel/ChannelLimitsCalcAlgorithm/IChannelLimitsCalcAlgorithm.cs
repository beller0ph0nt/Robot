
namespace Robot.TransportStructures
{
    public interface IChannelLimitsCalcAlgorithm
    {
        void CalcLimits(IChannelLimit lowerLimit, IChannelLimit upperLimit, IBlock firstBlock, IBlock secondBlock);
    }
}
