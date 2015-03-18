using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NinjaTrader.Data;

namespace TSAnalisator.Model.Converter
{
    public interface IConverter
    {
        void Convert(MarketDataEventArgs e);
    }
}
