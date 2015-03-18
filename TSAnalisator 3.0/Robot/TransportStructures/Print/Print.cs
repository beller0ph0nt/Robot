using System;

namespace Robot.TransportStructures
{
    public class Print : IPrint
    {
        #region Свойства

        public DateTime Time { get; set; }
        public double Price { get; set; }
        public long Volume { get; set; }
        public PrintType Type { get; set; }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public Print()
        {
            Time = new DateTime();
            Price = 0;
            Volume = 0;
            Type = PrintType.Unknown;
        }

        public Print(DateTime time, double price, long volume)
        {
            Time = new DateTime(time.Ticks);
            Price = price;
            Volume = volume;
            Type = PrintType.Unknown;
        }

        /// <summary>
        /// Конструктор копий
        /// </summary>
        /// <param name="prevPrint">Предыдущий экземпляр</param>
        public Print(IPrint prevPrint)
        {
            Time = prevPrint.Time;
            Price = prevPrint.Price;
            Volume = prevPrint.Volume;
            Type = prevPrint.Type;
        }

        /// <summary>
        /// Метод, вычисляющий тип принта
        /// </summary>
        /// <param name="bid">Цена лечшего покупателя</param>
        /// <param name="ask">Цена лучшего продавца</param>
        public void CalcPrintType(double bid, double ask)
        {
            if (Price == bid)
                Type = PrintType.AtBid;
            else if (Price == ask)
                Type = PrintType.AtAsk;
            else if (bid < Price && Price < ask)
                Type = PrintType.Between;
            else if (Price < bid)
                Type = PrintType.BelowBid;
            else if (ask < Price)
                Type = PrintType.AboveAsk;
            else
                Type = PrintType.Unknown;
        }

        public override string ToString()
        {
            return string.Format("PRINT time: {0:dd.MM.yyy HH:mm:ss.fff} dayOfYear: {1} month: {2} dayOfMonth: {3} dayOfWeek: {4} hour: {5} price: {6} volume: {7} type: {8}",
                Time,
                Time.DayOfYear,
                Time.Month,
                Time.Day,
                Time.DayOfWeek,
                Time.Hour,
                Price,
                Volume,
                Type);
        }
    }
}
