using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSAnalisator.Model.PrintStructures
{
    public class ChannelLimit
    {
        private double _k;
        private double _a;
        private double _b;

        #region Свойства
        /// <summary>
        /// Коэффициент наклона
        /// </summary>
        public double K
        {
            get { return _k; }
            set { _k = value; }
        }

        /// <summary>
        /// Смещение по оси x
        /// </summary>
        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        /// <summary>
        /// Смещение по оси y
        /// </summary>
        public double B
        {
            get { return _b; }
            set { _b = value; }
        }
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="k">Коэффициент наклона</param>
        /// <param name="a">Смещение по оси x</param>
        /// <param name="b">Смещение по оси y</param>
        public ChannelLimit(double k, double a, double b)
        {
            K = k;
            A = a;
            B = b;
        }

        /// <summary>
        /// Функция f(x) = k * (x + a) + b
        /// </summary>
        /// <param name="x">Параметр функции</param>
        /// <returns>Результат функции</returns>
        public double F(DateTime x)
        {
            return (K * (x.ToBinary() + A) + B); 
        }
    }
}
