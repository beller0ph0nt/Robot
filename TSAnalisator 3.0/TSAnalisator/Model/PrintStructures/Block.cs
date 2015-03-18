using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Debugging;

namespace TSAnalisator.Model.PrintStructures
{
    public class Block : IDisposable
    {
        private List<Impulse> _impulses;
        private PrintType _type;
        private double _high;
        private double _low;
        private TimeSpan _duration;
        private DateTime _firstPrintTime;
        private double _speed;
        private long _volume;

        #region Свойства
        /// <summary>
        /// Список импульсов
        /// </summary>
        public List<Impulse> Impulses { get { return _impulses; } private set { _impulses = value; } }

        /// <summary>
        /// Длительность блока
        /// </summary>
        public TimeSpan Duration { get { return _duration; } private set { _duration = value; } }

        /// <summary>
        /// Скорость блока
        /// </summary>
        public double Speed { get { return _speed; } private set { _speed = value; } }

        /// <summary>
        /// Тип блока
        /// </summary>
        public PrintType Type { get { return _type; } private set { _type = value; } }

        /// <summary>
        /// Суммарный объем всех принтов, прошедших в блоке
        /// </summary>
        public long Volume { get { return _volume; } private set { _volume = value; } }

        /// <summary>
        /// Наибольшая цена в блоке
        /// </summary>
        public double High { get { return _high; } private set { _high = value; } }

        /// <summary>
        /// Наименьшая цена в блоке
        /// </summary>
        public double Low { get { return _low; } private set { _low = value; } }
        #endregion

        public Block(PrintType type)
        {
            Log.Write(" Создаем НОВЫЙ блок");

            Type = type;
            Impulses = new List<Impulse>();
            Duration = new TimeSpan(0);
            Speed = 0.0;
            Volume = 0;
            High = 0.0;
            Low = 0.0;
        }

        public void Add(Print print)
        {
            try
            {
                if (Impulses.Count > 0)
                {
                    if (Impulses[0].IsPrintInImpulse(print) == false)
                        Impulses.Insert(0, new Impulse(print.Price, print.Type));
                }
                else
                    Impulses.Insert(0, new Impulse(print.Price, print.Type));

                Impulses[0].Add(print);

                // Перерасчет параметров блока
                CalcBlockProperties(print);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPrintInBlock(Print print)
        {
            try
            {
                if (print.Type == Type)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcBlockProperties(Print print)
        {
            if (Impulses.Count > 0)
            {
                _volume += print.Volume;

                // Первый принт в блоке
                if (Impulses.Count == 1 && Impulses[0].Prints.Count == 1)
                {
                    Low = High = print.Price;
                    Type = print.Type;
                    _firstPrintTime = print.Time;
                }
                else
                {
                    if (print.Price < Low)
                        Low = print.Price;
                    else if (print.Price > High)
                        High = print.Price;

                    Duration = new TimeSpan(print.Time.Ticks - _firstPrintTime.Ticks);
                    _speed = (High - Low) / Duration.Ticks;
                }

                if (Impulses.Count > 1)
                {
                    if (Type == PrintType.AtAsk && Impulses[0].Prints[Impulses[0].Prints.Count - 1].Price < Impulses[1].Prints[0].Price)
                        Impulses[0].BidVanish = true;
                    else if (Type == PrintType.AtBid && Impulses[0].Prints[Impulses[0].Prints.Count - 1].Price > Impulses[1].Prints[0].Price)
                        Impulses[0].AskVanish = true;
                }
            }
        }

        public void Dispose()
        {
            try
            {
                foreach (Impulse impulse in _impulses)
                    impulse.Dispose();

                _impulses.Clear();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
