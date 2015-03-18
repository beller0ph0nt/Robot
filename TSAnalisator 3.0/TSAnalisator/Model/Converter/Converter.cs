using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSAnalisator.Model.Analisator.Recorder;

namespace TSAnalisator.Model.Converter
{
    public class Converter : IConverter
    {
        private IRecorder _recorder;

        public Converter(IRecorder recorder)
        {
            _recorder = recorder;
        }

        public void Convert(NinjaTrader.Data.MarketDataEventArgs e)
        {
        }
    }
}
