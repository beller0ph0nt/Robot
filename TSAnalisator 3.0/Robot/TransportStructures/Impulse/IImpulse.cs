using System;
using System.Collections.Generic;

namespace Robot.TransportStructures
{
    public interface IImpulse
    {
        List<IPrint> Prints { get; }
        bool BidVanish { get; set; }
        bool AskVanish { get; set; }
        IPoint High { get; set; }
        IPoint Low { get; set; }
        TimeSpan Duration { get; set; }
        double Price { get; }
        long Volume { get; set; }
        PrintType Type { get; }

        void Push(IPrint print);
        bool IsPrintInImpulse(IPrint print);
        string ToString();
    }
}
