using System.Threading;
using NinjaTrader.Data;

namespace TSAnalisator.Model
{
    public class ThreadParameter
    {
        public MarketDataEventArgs _e { get; private set; }
        public AutoResetEvent _goingAheadThread { get; private set; }
        public AutoResetEvent _currentThread { get; private set; }

        public ThreadParameter(MarketDataEventArgs e, AutoResetEvent goingAheadThread, AutoResetEvent currentThread)
        {
            _e = e;
            _goingAheadThread = goingAheadThread;
            _currentThread = currentThread;
        }
    }
}
