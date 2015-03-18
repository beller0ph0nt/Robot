using NinjaTrader.Data;

namespace TSAnalisator.Model.TimeAndSales
{
    public interface ITimeAndSales
    {
        long TotalVolume { get; }
        bool Msec { get; set; }

        

        //Color AskVanishFontColor { get; set; }
        //Color AskVanishGroundColor { get; set; }
        //Color BidVanishFontColor { get; set; }
        //Color BidVanishGroundColor { get; set; }

        void NewMarketData(MarketDataEventArgs e);

        //event Action<Print> NewPrint;
    }
}
