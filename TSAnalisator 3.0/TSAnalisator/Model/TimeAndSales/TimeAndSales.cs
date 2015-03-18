using System;
using System.Threading;
using NinjaTrader.Data;
using TSAnalisator.Model.Analisator;
using TSAnalisator.Model.DrawPrintsBuffer;
using TSAnalisator.View;

namespace TSAnalisator.Model.TimeAndSales
{
    public class TimeAndSales : ITimeAndSales
    {
        private object _locker = new object();
        private bool _msec;
        private double _ask;
        private double _bid;
        private long _totalVolume;

        private readonly IDrawPrintsBuffer _drawPrintsBuffer;
        private readonly IAnalisator _analisator;
        private readonly IView _view;

        private AutoResetEvent _goingAheadThread;   // Впередиидущий поток
        private AutoResetEvent _currentThread;      // Текущий поток

        #region Свойства

        public long TotalVolume { get { return _totalVolume; } private set { _totalVolume = value; } }
        public bool Msec { get { return _msec; } set { _msec = value; } }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="i">Инструмент</param>
        public TimeAndSales(IView view, IDrawPrintsBuffer drawPrintsBuffer, IAnalisator analisator)
        {
            //ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);
            //ThreadPool.SetMaxThreads(1, 1);
            
            _ask = 0.0;
            _bid = 0.0;
            _msec = true;
            _totalVolume = 0;

            _drawPrintsBuffer = drawPrintsBuffer;
            _analisator = analisator;
            _view = view;

            _goingAheadThread = null;
            _currentThread = null;
        }

        ~TimeAndSales()
        {
            if (_currentThread != null)
                _currentThread.WaitOne();
        }

        private void NewMarketDataAsync(object obj)
        {
            try
            {
                DateTime tmpPrintTime = new DateTime(DateTime.Now.Ticks);
                var tmpPar = obj as ThreadParameter;

                if (tmpPar._goingAheadThread != null)
                    tmpPar._goingAheadThread.WaitOne();

                if (tmpPar._e.MarketDataType == MarketDataType.Last)
                {
                    Print print = new Print();

                    print.Price = tmpPar._e.Price;
                    print.Time = (Msec) ? tmpPar._e.Time.AddMilliseconds(tmpPrintTime.Millisecond) : tmpPar._e.Time;
                    print.Volume = tmpPar._e.Volume;
                    print.Type = Print.CalcPrintType(_bid, _ask, print.Price);

                    _totalVolume += print.Volume;

                    _drawPrintsBuffer.PushPrint(print);

                    _analisator.Add(print);

                    _view.Draw(_drawPrintsBuffer.Prints);
                    _view.Draw(_totalVolume);
                }
                else if (tmpPar._e.MarketDataType == MarketDataType.Bid)
                    _bid = tmpPar._e.Price;
                else if (tmpPar._e.MarketDataType == MarketDataType.Ask)
                    _ask = tmpPar._e.Price;

                if (tmpPar._currentThread != null)
                    tmpPar._currentThread.Set();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Обрабатывает новые рыночные данные
        /// </summary>
        /// <param name="e">Рыночные данные</param>
        public void NewMarketData(MarketDataEventArgs e)
        {
            try
            {
                //_goingAheadThread = _currentThread;
                //_currentThread = new AutoResetEvent(false);
                //ThreadPool.QueueUserWorkItem(NewMarketDataAsync,
                //                             new ThreadParameter(new MarketDataEventArgs(e.MarketData,
                //                                                                         e.Error,
                //                                                                         e.NativeError,
                //                                                                         e.MarketDataType,
                //                                                                         e.Price,
                //                                                                         e.Volume,
                //                                                                         e.Time),
                //                                                 _goingAheadThread,
                //                                                 _currentThread));
                NewMarketDataAsync(new ThreadParameter(e, _goingAheadThread, _currentThread));
            }
            catch (Exception ex) { throw ex; }
        }

        //public event Action<Print> NewPrint;
    }
}
