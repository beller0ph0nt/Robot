using System;
using NinjaTrader.Data;
using Robot.DataGateway;
using Robot.TransportStructures;

namespace Robot.Accumulator
{
    public class ChannelAccumulator : IAccumulator
    {
        private double _bid = 0;
        private double _ask = 0;
        private IImpulse _impulse = new Impulse();
        private IBlock _block = new Block();
        private IChannel _channel = new Channel();
        private IDataGateway _dataGateway;

        public ChannelAccumulator()
        {
            _dataGateway = new ChannelDbDataGateway();
        }

        public ChannelAccumulator(string DataBase, string UserId, string Password, string Server, string Port)
        {
            _dataGateway = new ChannelDbDataGateway(DataBase, UserId, Password, Server, Port);
        }

        public void Push(MarketDataEventArgs e)
        {
            try
            {
                if (e.MarketDataType == MarketDataType.Last)
                {
                    IPrint print = new Print();

                    print.Price = e.Price;
                    print.Time = e.Time;
                    print.Volume = e.Volume;
                    print.CalcPrintType((_bid == 0) ? e.MarketData.Bid.Price : _bid,
                                        (_ask == 0) ? e.MarketData.Ask.Price : _ask);

                    if (_impulse.IsPrintInImpulse(print))
                        _impulse.Push(print);
                    else
                    {
                        if (_block.IsImpulseInBlock(_impulse))
                            _block.Push(_impulse);
                        else
                        {
                            if (_channel.IsBlockInChannel(_block))
                                _channel.Push(_block);
                            else
                            {
                                _dataGateway.Write(_channel);

                                _channel = new Channel();
                                _channel.Push(_block);
                            }

                            _block = new Block();
                            _block.Push(_impulse);
                        }

                        _impulse = new Impulse();
                        _impulse.Push(print);
                    }
                }
                else if (e.MarketDataType == MarketDataType.Bid)
                    _bid = e.Price;
                else if (e.MarketDataType == MarketDataType.Ask)
                    _ask = e.Price;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
