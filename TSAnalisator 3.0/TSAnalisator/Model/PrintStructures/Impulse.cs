using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Debugging;

namespace TSAnalisator.Model.PrintStructures
{
    public class Impulse : IDisposable
    {
        private List<Print> _prints;
        private double _price;
        private PrintType _type;
        private TimeSpan _duration;
        private long _volume;
        private bool _bidVanish;
        private bool _askVanish;

        #region Свойства

        public List<Print> Prints { get { return _prints; } private set { _prints = value; } }
        public double Price { get { return _price; } private set { _price = value; } }
        public TimeSpan Duration { get { return _duration; } private set { _duration = value; } }
        public long Volume { get { return _volume; } private set { _volume = value; } }
        public PrintType Type { get { return _type; } private set { _type = value; } }
        public bool BidVanish { get { return _bidVanish; } set { _bidVanish = value; } }
        public bool AskVanish { get { return _askVanish; } set { _askVanish = value; } }

        #endregion

        public Impulse(double price, PrintType type)
        {
            Log.Write("  Создаем НОВЫЙ импульс");

            Price = price;
            Type = type;
            Prints = new List<Print>();
            Duration = new TimeSpan(0);
            Volume = 0;
            BidVanish = false;
            AskVanish = false;
        }

        public void Add(Print print)
        {
            try
            {
                Log.Write(string.Format("   Добавляем принт в ТЕКУЩИЙ импульс {0} {1} {2}", print.Time.ToString(), print.Price, print.Volume));

                Prints.Insert(0, print);

                // Перерасчет параметров импульса
                CalcImpulseProperties();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPrintInImpulse(Print print)
        {
            try
            {
                if (print.Type == Type && print.Price == Price)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcImpulseProperties()
        {
            if (_prints.Count > 0)
            {
                _volume += _prints[0].Volume;
                _duration = new TimeSpan(_prints[0].Time.Ticks - _prints[_prints.Count - 1].Time.Ticks);
            }
        }

        public void Dispose()
        {
            try { _prints.Clear(); }
            catch (Exception ex) { throw ex; }
        }
    }
}
