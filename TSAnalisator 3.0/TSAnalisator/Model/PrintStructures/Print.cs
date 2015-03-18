using System;

namespace TSAnalisator.Model
{
    /// <summary>
    /// Типы принтов
    /// </summary>
    public enum PrintType
    {
        AtBid,
        AtAsk,
        BelowBid,
        AboveAsk,
        Between,
        Unknown
    }

    public class Print
    {
        private double _price;
        private DateTime _time;
        private PrintType _type;
        private long _volume;

        public double Price
        {
            get { return _price; }
            set 
            { 
                _price = value;
                PrintChanged();
            }
        }

        public DateTime Time
        {
            get { return _time; }
            set 
            { 
                _time = value;
                PrintChanged();
            }
        }

        public PrintType Type
        {
            get { return _type; }
            set 
            { 
                _type = value;
                PrintChanged();
            }
        }

        public long Volume
        {
            get { return _volume; }
            set 
            { 
                _volume = value;
                PrintChanged();
            }
        }

        protected virtual void PrintChanged()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Print()
        {
            Price = 0.0;
            Time = new DateTime();
            Type = PrintType.Unknown;
            Volume = 0;
        }

        /// <summary>
        /// Конструктор копий
        /// </summary>
        /// <param name="prevPrint">Предыдущий экземпляр</param>
        public Print(Print prevPrint)
        {
            Price = prevPrint._price;
            Time = prevPrint._time;
            Type = prevPrint._type;
            Volume = prevPrint._volume;
        }
        
        /// <summary>
        /// Вычисляет тип принта для текущего экземпяра
        /// </summary>
        /// <param name="bid">Цена лечшего покупателя</param>
        /// <param name="ask">Цена лучшего продавца</param>
        /// <param name="price">Цена по которой прошел принт</param>
        /// <returns>Тип принта</returns>
        public static PrintType CalcPrintType(double bid, double ask, double price)
        {
            try
            {
                if (price == bid)
                    return PrintType.AtBid;
                else if (price == ask)
                    return PrintType.AtAsk;
                else if (bid < price && price < ask)
                    return PrintType.Between;
                else if (price < bid)
                    return PrintType.BelowBid;
                else if (ask < price)
                    return PrintType.AboveAsk;
                else
                    return PrintType.Unknown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
