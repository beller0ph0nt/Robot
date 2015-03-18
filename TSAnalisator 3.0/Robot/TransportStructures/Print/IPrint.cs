using System;

namespace Robot.TransportStructures
{
    /// <summary>
    /// Типы принтов
    /// </summary>
    public enum PrintType
    {
        AtBid = 1,
        AtAsk = 2,
        BelowBid = 3,
        AboveAsk = 4,
        Between = 5,
        Unknown = 0
    }

    public interface IPrint
    {
        //int Id { get; set; }
        DateTime Time { get; set; }
        double Price { get; set; }
        long Volume { get; set; }
        PrintType Type { get; set; }

        void CalcPrintType(double bid, double ask);
        string ToString();
    }
}
