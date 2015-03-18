using System.Collections.Generic;
using System.Drawing;
using NinjaTrader.Cbi;
using TSAnalisator.Model.PrintStructures;

namespace TSAnalisator.Model.DrawPrintsBuffer
{
    public interface IDrawPrintsBuffer
    {
        Instrument Instrument { get; }
        List<DrawPrint> Prints { get; }
        bool CMESplit { get; set; }
        Font Font { get; set; }
        Color AtAskFontColor { get; set; }
        Color AtAskGroundColor { get; set; }
        Color AboveAskFontColor { get; set; }
        Color AboveAskGroundColor { get; set; }
        Color AtBidFontColor { get; set; }
        Color AtBidGroundColor { get; set; }
        Color BelowBidFontColor { get; set; }
        Color BelowBidGroundColor { get; set; }
        Color BetweenFontColor { get; set; }
        Color BetweenGroundColor { get; set; }
        Color UnknownFontColor { get; set; }
        Color UnknownGroundColor { get; set; }
        string TimePattern { get; set; }

        void PushPrint(Print print);
    }
}
