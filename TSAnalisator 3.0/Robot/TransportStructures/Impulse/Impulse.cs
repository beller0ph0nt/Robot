using System;
using System.Collections.Generic;

namespace Robot.TransportStructures
{
    /// <summary>
    /// Класс импульса. Импульс группирует в себе принты по типу и цене
    /// </summary>
    public class Impulse : IImpulse
    {
        #region Свойства

        public List<IPrint> Prints { get; set; }
        public bool BidVanish { get; set; }
        public bool AskVanish { get; set; }
        public IPoint High { get; set; }
        public IPoint Low { get; set; }
        public TimeSpan Duration { get; set; }
        public double Price { get; set; }
        public long Volume { get; set; }
        public PrintType Type { get; set; }

        #endregion

        public Impulse()
        {
            Prints = new List<IPrint>();
            BidVanish = false;
            AskVanish = false;
            High = new Point(0, 0);
            Low = new Point(0, 0);
            Duration = new TimeSpan(0);
            Price = 0;
            Volume = 0;
            Type = PrintType.Unknown;
        }

        public void Push(IPrint print)
        {
            try
            {
                Prints.Insert(0, print);

                // Перерасчет параметров импульса
                if (Prints.Count == 1)
                {
                    High.X = Low.X = print.Time.Ticks;
                    High.Y = Low.Y = print.Price;
                }
                else
                {
                    if (print.Price > High.Y)
                    {
                        High.X = print.Time.Ticks;
                        High.Y = print.Price;
                    }
                    else if (print.Price < Low.Y)
                    {
                        Low.X = print.Time.Ticks;
                        Low.Y = print.Price;
                    }
                }

                Duration = new TimeSpan(Prints[0].Time.Ticks - Prints[Prints.Count - 1].Time.Ticks);
                Price = print.Price;
                Volume += print.Volume;
                Type = print.Type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPrintInImpulse(IPrint print)
        {
            if ((Type == PrintType.Unknown) ||
                (print.Type == Type && print.Price == Price))
                return true;

            return false;
        }

        public override string ToString()
        {
            return string.Format("IMPULSE bidVanish: {0} askVanish: {1} high: {2} low: {3} duration: {4} price: {5} volume: {6} type: {7} prints.count: {8}",
                BidVanish,
                AskVanish,
                High.ToString(),
                Low.ToString(),
                Duration,
                Price,
                Volume,
                Type,
                Prints.Count);
        }
    }
}
