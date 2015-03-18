using System;
using System.Drawing;
using NinjaTrader.Cbi;

namespace TSAnalisator.Model.PrintStructures
{
    public class DrawPrint : Print
    {
        private Color _fontColor;
        private Color _groundColor;
        private string _outputPrice;
        private Font _printFont;
        private string _timePattern;
        private string _info;
        private Bitmap _bitmap;

        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        public Color GroundColor
        {
            get { return _groundColor; }
            set { _groundColor = value; }
        }

        public string OutputPrice
        {
            get { return _outputPrice; }
            set { _outputPrice = value; }
        }

        public Font PrintFont
        {
            get { return _printFont; }
            set { _printFont = value; }
        }

        public string TimePattern
        {
            get { return _timePattern; }
            set { _timePattern = value; }
        }

        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DrawPrint()
        {
            FontColor = Color.Black;
            GroundColor = Color.White;
            OutputPrice = "";
            PrintFont = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);
            TimePattern = "hh:mm:ss";
            Info = "";
            Bitmap = null;
        }

        /// <summary>
        /// Конструктор копий
        /// </summary>
        /// <param name="prevPrint">Предыдущий экземпляр</param>
        public DrawPrint(Print prevPrint)
        {
            Price = prevPrint.Price;
            Time = prevPrint.Time;
            Volume = prevPrint.Volume;
            Type = prevPrint.Type;
        }

        /// <summary>
        /// Конструктор копий
        /// </summary>
        /// <param name="prevDrawPrint">Предыдущий экземпляр</param>
        public DrawPrint(DrawPrint prevDrawPrint)
        {
            Price = prevDrawPrint.Price;
            Time = prevDrawPrint.Time;
            Volume = prevDrawPrint.Volume;
            Type = prevDrawPrint.Type;
            FontColor = prevDrawPrint._fontColor;
            GroundColor = prevDrawPrint._groundColor;
            OutputPrice = prevDrawPrint._outputPrice;
            PrintFont = prevDrawPrint._printFont;
            TimePattern = prevDrawPrint._timePattern;
            Info = prevDrawPrint.Info;
            Bitmap = prevDrawPrint.Bitmap;
        }

        protected override void PrintChanged()
        {
            if (Bitmap != null)
                Bitmap.Dispose();

            Bitmap = null;
        }

        /// <summary>
        /// Вычисляет цену, выводимую на экран, для текущего экземпляра
        /// </summary>
        /// <param name="i">Инструмент на основе которого вычисляется цена</param>
        /// <param name="price">Вычисляемая цена</param>
        /// <returns>Цена, выводимая на экран</returns>
        public static string CalcOutputPrice(Instrument i, double price)
        {
            try
            {
                string outputPrice;
                string[] tmpSplit;


                if (i.MasterInstrument.TickSize == 0.03125)
                    outputPrice = Math.Truncate(price) + "'" + Convert.ToString((price - Math.Truncate(price)) / i.MasterInstrument.TickSize).Split('.', ',')[0].PadLeft(2, '0');
                else if (i.MasterInstrument.TickSize == 0.015625)
                {
                    outputPrice = Math.Truncate(price) + "'";
                    tmpSplit = Convert.ToString(((price - Math.Truncate(price)) / i.MasterInstrument.TickSize) / 2).Split('.', ',');
                    outputPrice += tmpSplit[0].PadLeft(2, '0') + (tmpSplit.Length > 1 ? tmpSplit[1] : "0");
                }
                else if (i.MasterInstrument.TickSize == 0.0078125)
                {
                    outputPrice = Math.Truncate(price) + "'";
                    tmpSplit = Convert.ToString(((price - Math.Truncate(price)) / i.MasterInstrument.TickSize) / 4).Split('.', ',');
                    outputPrice += tmpSplit[0].PadLeft(2, '0') + (tmpSplit.Length > 1 ? tmpSplit[1] : "00");
                }
                else
                    outputPrice = price.ToString();

                return outputPrice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
