using System;
using NinjaTrader.Data;
using Robot.Accumulator;
using Debugging;

namespace Robot
{
    public class ChannelRobot : IRobot
    {
        private IAccumulator _accumulator;

        public ChannelRobot()
        {
            _accumulator = new ChannelAccumulator();
        }

        public ChannelRobot(string DataBase, string UserId, string Password, string Server, string Port)
        {
            _accumulator = new ChannelAccumulator(DataBase, UserId, Password, Server, Port);
        }

        public void Learn()
        {
        }

        public void Save(Object obj)
        {
            Log.Write("ChannelRobot.Save Begin");

            if (obj is MarketDataEventArgs)
            {
                MarketDataEventArgs marketData = obj as MarketDataEventArgs;
                MarketDataEventArgs marketDataClone = new MarketDataEventArgs(
                    marketData.MarketData,
                    marketData.Error,
                    marketData.NativeError,
                    marketData.MarketDataType,
                    marketData.Price,
                    marketData.Volume,
                    marketData.Time);

                _accumulator.Push(marketDataClone);
            }
            else
                throw new Exception("Некорректный тип данных");

            Log.Write("ChannelRobot.Save End");
        }

        public void Work(Object obj)
        {
            if (obj is MarketDataEventArgs)
            {
            }
            else
                throw new Exception("Некорректный тип данных");
        }
    }
}
