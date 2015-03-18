using System;
using System.Collections.Generic;

namespace Robot.TransportStructures
{
    public class Channel : IChannel
    {
        private IChannelLimitsCalcAlgorithm _channelLimitsCalcAlgorithm;

        #region Свойства

        public virtual List<IBlock> Blocks { get; set; }
        public IChannelLimit UpperLimit { get; set; }
        public IChannelLimit LowerLimit { get; set; }
        public ChannelType Type { get; set; }

        #endregion

        public Channel()
        {
            Blocks = new List<IBlock>();
            UpperLimit = new ChannelLimit(0, 0);
            LowerLimit = new ChannelLimit(0, 0);
            Type = ChannelType.Unknown;

            _channelLimitsCalcAlgorithm = new EmptyLimitsCalcAlgorithm();
        }

        #region Методы

        public void Push(IBlock block)
        {
            try
            {
                Blocks.Insert(0, block);

                // Перерасчет параметров канала
                if (Blocks.Count == 3)
                {
                    if (Type == ChannelType.Unknown)
                    {
                        if (Blocks[1].Low.Y > Blocks[2].Low.Y)          // Повышающиеся минимумы
                            Type = ChannelType.DoubleMinUpTrand;
                        else if (Blocks[1].High.Y > Blocks[2].High.Y)   // Повышающиеся максимумы
                            Type = ChannelType.DoubleMaxUpTrand;
                        else if (Blocks[1].High.Y < Blocks[2].High.Y)   // Понижающиеся максимумы
                            Type = ChannelType.DoubleMaxDownTrand;
                        else if (Blocks[1].Low.Y < Blocks[2].Low.Y)     // Понижающиеся минимумы
                            Type = ChannelType.DoubleMinDownTrand;
                        else if (Blocks[1].High.Y == Blocks[2].High.Y)  // Два максимума
                            Type = ChannelType.DoubleMaxConsolidation;
                        else if (Blocks[1].Low.Y == Blocks[2].Low.Y)    // Два минимума
                            Type = ChannelType.DoubleMinConsolidation;

                        if (Type == ChannelType.DoubleMaxUpTrand || Type == ChannelType.DoubleMaxDownTrand)
                            _channelLimitsCalcAlgorithm = new DoubleMaxTrandAlgorithm();
                        else if (Type == ChannelType.DoubleMinUpTrand || Type == ChannelType.DoubleMinDownTrand)
                            _channelLimitsCalcAlgorithm = new DoubleMinTrandAlgorithm();
                        else if (Type == ChannelType.DoubleMaxConsolidation || Type == ChannelType.DoubleMinConsolidation)
                            _channelLimitsCalcAlgorithm = new ConsolidationAlgorithm();

                        _channelLimitsCalcAlgorithm.CalcLimits(LowerLimit, UpperLimit, Blocks[2], Blocks[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsBlockInChannel(IBlock block)
        {
            // Если блок полностью находится в канале
            if ((Type == ChannelType.Unknown) ||
                (UpperLimit.F(block.High.X) >= block.High.Y && block.Low.Y >= LowerLimit.F(block.Low.X)))
                return true;

            return false;
        }

        public override string ToString()
        {
            return string.Format("CHANNEL upperLimit: {0} lowerLimit: {1} type: {2} bloks.count: {3}",
                UpperLimit.ToString(),
                LowerLimit.ToString(),
                Type,
                Blocks.Count);
        }

        #endregion
    }
}