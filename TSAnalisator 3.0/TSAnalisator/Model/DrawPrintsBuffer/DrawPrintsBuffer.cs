using System;
using System.Collections.Generic;
using System.Drawing;
using NinjaTrader.Cbi;
using TSAnalisator.Model.PrintStructures;

namespace TSAnalisator.Model.DrawPrintsBuffer
{
    public class DrawPrintsBuffer : IDrawPrintsBuffer
    {
        private Instrument _instrument;     // Инструмент
        private bool _cmeSplit;             // Флаг группировки CMESplit
        private int _maxPrintCount;         // Максимальное кол-во элементов в списке
        private List<DrawPrint> _prints;    // Список принтов
        private Font _font;                 // Шрифт принтов
        private Color _atAskFontColor;      // Цвет шрифта AtAsk
        private Color _atAskGroundColor;    // Цвет фона AtAsk
        private Color _aboveAskFontColor;   // Цвет шрифта AboveAsk
        private Color _aboveAskGroundColor; // Цвет фона AboveAsk
        private Color _atBidFontColor;      // Цвет шрифта AtBid
        private Color _atBidGroundColor;    // Цвет фона AtBid
        private Color _belowBidFontColor;   // Цвет шрифта BelowBid
        private Color _belowBidGroundColor; // Цвет фона BelowBid
        private Color _betweenFontColor;    // Цвет шрифта Between
        private Color _betweenGroundColor;  // Цвет фона Between
        private Color _unknownFontColor;    // Цвет шрифта Unknown
        private Color _unknownGroundColor;  // Цвет фона Unknown
        private string _timePattern;        // Шаблон времени

        #region Свойства

        public Instrument Instrument { get { return _instrument; } private set { _instrument = value; } }
        public List<DrawPrint> Prints { get { return _prints; } private set { _prints = value; } }
        public bool CMESplit { get { return _cmeSplit; } set { _cmeSplit = value; } }
        public Color AtAskFontColor { get { return _atAskFontColor; } set { _atAskFontColor = value; } }
        public Color AtAskGroundColor { get { return _atAskGroundColor; } set { _atAskGroundColor = value; } }
        public Color AboveAskFontColor { get { return _aboveAskFontColor; } set { _aboveAskFontColor = value; } }
        public Color AboveAskGroundColor { get { return _aboveAskGroundColor; } set { _aboveAskGroundColor = value; } }
        public Color AtBidFontColor { get { return _atBidFontColor; } set { _atBidFontColor = value; } }
        public Color AtBidGroundColor { get { return _atBidGroundColor; } set { _atBidGroundColor = value; } }
        public Color BelowBidFontColor { get { return _belowBidFontColor; } set { _belowBidFontColor = value; } }
        public Color BelowBidGroundColor { get { return _belowBidGroundColor; } set { _belowBidGroundColor = value; } }
        public Color BetweenFontColor { get { return _betweenFontColor; } set { _betweenFontColor = value; } }
        public Color BetweenGroundColor { get { return _betweenGroundColor; } set { _betweenGroundColor = value; } }
        public Color UnknownFontColor { get { return _unknownFontColor; } set { _unknownFontColor = value; } }
        public Color UnknownGroundColor { get { return _unknownGroundColor; } set { _unknownGroundColor = value; } }
        public string TimePattern { get { return _timePattern; } set { _timePattern = value; } }
        private int MaxPrintCount { get { return _maxPrintCount; } set { _maxPrintCount = value; } }
        public Font Font { get { return _font; } set { _font = value; } }

        #endregion

        public DrawPrintsBuffer(Instrument i)
        {
            Instrument = i;
            Prints = new List<DrawPrint>();
            CMESplit = true;
            Font = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);
            MaxPrintCount = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / _font.Height;
            AtAskFontColor = Color.Lime;
            AtAskGroundColor = Color.Black;
            AboveAskFontColor = Color.Lime;
            AboveAskGroundColor = Color.Black;
            AtBidFontColor = Color.Red;
            AtBidGroundColor = Color.Black;
            BelowBidFontColor = Color.Red;
            BelowBidGroundColor = Color.Black;
            BetweenFontColor = Color.White;
            BetweenGroundColor = Color.Black;
            UnknownFontColor = Color.White;
            UnknownGroundColor = Color.Black;
            TimePattern = "HH:mm:ss";
        }

        /// <summary>
        /// Вставляет принт в буффер
        /// </summary>
        /// <param name="print">Принт</param>
        public void PushPrint(Print print)
        {
            try
            {
                DrawPrint tmpDrawPrint = new DrawPrint(print);

                tmpDrawPrint.TimePattern = TimePattern;
                tmpDrawPrint.OutputPrice = DrawPrint.CalcOutputPrice(Instrument, tmpDrawPrint.Price);
                tmpDrawPrint.PrintFont = Font;

                if (tmpDrawPrint.Type == PrintType.AtBid)
                {
                    tmpDrawPrint.GroundColor = AtBidGroundColor;
                    tmpDrawPrint.FontColor = AtBidFontColor;
                }
                else if (tmpDrawPrint.Type == PrintType.AtAsk)
                {
                    tmpDrawPrint.GroundColor = AtAskGroundColor;
                    tmpDrawPrint.FontColor = AtAskFontColor;
                }
                else if (tmpDrawPrint.Type == PrintType.BelowBid)
                {
                    tmpDrawPrint.GroundColor = BelowBidGroundColor;
                    tmpDrawPrint.FontColor = BelowBidFontColor;
                }
                else if (tmpDrawPrint.Type == PrintType.AboveAsk)
                {
                    tmpDrawPrint.GroundColor = AboveAskGroundColor;
                    tmpDrawPrint.FontColor = AboveAskFontColor;
                }
                else if (tmpDrawPrint.Type == PrintType.Between)
                {
                    tmpDrawPrint.GroundColor = BetweenGroundColor;
                    tmpDrawPrint.FontColor = BetweenFontColor;
                }
                else if (tmpDrawPrint.Type == PrintType.Unknown)
                {
                    tmpDrawPrint.GroundColor = UnknownGroundColor;
                    tmpDrawPrint.FontColor = UnknownFontColor;
                }

                PushPrint(tmpDrawPrint);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Вставляет принт в буффер
        /// </summary>
        /// <param name="print">Принт</param>
        public void PushPrint(DrawPrint print)
        {
            try
            {
                if (CMESplit && Prints.Count != 0)
                {
                    if (Prints[0].Time == print.Time && Prints[0].Price == print.Price && Prints[0].Type == print.Type)
                    {
                        Prints[0].Volume += print.Volume;

                        return;
                    }
                }

                Prints.Insert(0, print);
                if (Prints.Count > _maxPrintCount)
                    Prints.RemoveAt(_maxPrintCount);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
