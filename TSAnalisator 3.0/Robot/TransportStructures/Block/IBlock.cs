using System;
using System.Collections.Generic;

namespace Robot.TransportStructures
{
    public interface IBlock
    {
        List<IImpulse> Impulses { get; }
        IPoint High { get; set; }
        IPoint Low { get; set; }
        TimeSpan Duration { get; set; }
        double Distance { get; set; }
        long Volume { get; set; }
        PrintType Type { get; }

        void Push(IImpulse impulse);
        bool IsImpulseInBlock(IImpulse impulse);
        string ToString();
    }
}
