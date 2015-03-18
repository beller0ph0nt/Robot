using NinjaTrader.Data;

namespace Robot.Accumulator
{
    /// <summary>
    /// Интерфейс накопителя биржевых данных
    /// </summary>
    public interface IAccumulator
    {
        void Push(MarketDataEventArgs e);
    }
}
