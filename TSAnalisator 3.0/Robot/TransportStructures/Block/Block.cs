using System;
using System.Collections.Generic;

namespace Robot.TransportStructures
{
    public class Block : IBlock
    {
        #region Свойства

        public List<IImpulse> Impulses { get; set; }
        public IPoint High { get; set; }
        public IPoint Low { get; set; }
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
        public long Volume { get; set; }
        public PrintType Type { get; set; }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public Block()
        {
            Impulses = new List<IImpulse>();
            High = new Point(0, 0);
            Low = new Point(0, 0);
            Duration = new TimeSpan(0);
            Distance = 0;
            Volume = 0;
            Type = PrintType.Unknown;
        }

        public void Push(IImpulse impulse)
        {
            try
            {
                Impulses.Insert(0, impulse);

                // Перерасчет параметров блока
                if (Impulses.Count == 1)
                {
                    High.X = impulse.High.X;
                    High.Y = impulse.High.Y;
                    Low.X = impulse.Low.X;
                    Low.Y = impulse.Low.Y;
                }
                else
                {
                    if (impulse.High.Y > High.Y)
                    {
                        High.X = impulse.High.X;
                        High.Y = impulse.High.Y;
                    }
                    else if (impulse.Low.Y < Low.Y)
                    {
                        Low.X = impulse.Low.X;
                        Low.Y = impulse.Low.Y;
                    }
                }

                if (Impulses.Count > 1)
                {
                    if (Type == PrintType.AtAsk && Impulses[0].Price < Impulses[1].Price)        // Идут покупки, но цена падает
                        Impulses[0].BidVanish = true;
                    else if (Type == PrintType.AtBid && Impulses[0].Price > Impulses[1].Price)   // Идут продажи, но цена растет
                        Impulses[0].AskVanish = true;
                }

                Duration += impulse.Duration;
                Distance = High.Y - Low.Y;
                Volume += impulse.Volume;
                Type = impulse.Type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsImpulseInBlock(IImpulse impulse)
        {
            if (Type == PrintType.Unknown ||
                Type == impulse.Type)
                return true;

            return false;
        }

        public override string ToString()
        {
            try
            {
                return string.Format("BLOCK high: {0} low: {1} duration: {2} distance: {3} volume: {4} type: {5} impulses.count: {6}",
                    High.ToString(),
                    Low.ToString(),
                    Duration,
                    Distance,
                    Volume,
                    Type,
                    Impulses.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
