using System.Collections.Generic;

namespace Robot.TransportStructures
{
    public enum ChannelType
    {
        Unknown = 0,
        DoubleMaxUpTrand = 1,
        DoubleMinUpTrand = 2,
        DoubleMaxDownTrand = 3,
        DoubleMinDownTrand = 4,
        DoubleMaxConsolidation = 5,
        DoubleMinConsolidation = 6
    }

    public interface IChannel
    {
        List<IBlock> Blocks { get; set; }
        IChannelLimit UpperLimit { get; set; }
        IChannelLimit LowerLimit { get; set; }
        ChannelType Type { get; set; }

        void Push(IBlock block);
        bool IsBlockInChannel(IBlock block);
        string ToString();
    }
}
