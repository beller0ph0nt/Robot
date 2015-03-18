using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Debugging;

namespace TSAnalisator.Model.PrintStructures
{
    /// <summary>
    /// Типы каналов
    /// </summary>
    public enum ChannelType
    {
        DoubleHighUpTrand,
        DoubleLowUpTrand,
        DoubleHighDownTrand,
        DoubleLowDownTrand,
        DoubleHighConsolidation,
        DoubleLowConsolidation,
        Unknown
    }

    public class Channel : IDisposable
    {
        private List<Block> _blocks;
        private ChannelType _type;
        private ChannelLimit _upperLimit;
        private ChannelLimit _lowerLimit;

        #region Свойства
        public List<Block> Blocks
        {
            get { return _blocks; }
            private set { _blocks = value; }
        }

        public ChannelType Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        public ChannelLimit UpperLimit
        {
            get { return _upperLimit; }
            private set { _upperLimit = value; }
        }

        public ChannelLimit LowerLimit
        {
            get { return _lowerLimit; }
            private set { _lowerLimit = value; }
        }
        #endregion

        public Channel()
        {
            Log.Write("Создаем НОВЫЙ канал");

            Blocks = new List<Block>();
            Type = ChannelType.Unknown;
            UpperLimit = new ChannelLimit(0.0, 0.0, 0.0);
            LowerLimit = new ChannelLimit(0.0, 0.0, 0.0);
        }

        /// <summary>
        /// Добавляет принта в канал
        /// </summary>
        /// <param name="print">Добавляемый принт</param>
        public void Add(Print print)
        {
            try
            {
                if (Blocks.Count > 0)
                {
                    if (Blocks[0].IsPrintInBlock(print) == false)
                        Blocks.Insert(0, new Block(print.Type));
                }
                else
                    Blocks.Insert(0, new Block(print.Type));

                Blocks[0].Add(print);
                
                // Перерасчет границ канала
                CalcChannelProperties();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Проверяет принадлежность принта к каналу
        /// </summary>
        /// <param name="print">Проверяемый принт</param>
        /// <returns>true - принт принадлежит каналу; false - принт не принадлежыт каналу</returns>
        public bool IsPrintInChannel(Print print)
        {
            try
            {
                // Если тип канала еще не определен, значит в нем не достаточно блоков, поэтому добавляем принт
                if (Type == ChannelType.Unknown)
                    return true;
                
                // Если цена принта расположена в канале, то добавляем принт
                if ((UpperLimit.F(print.Time) >= print.Price) && (print.Price >= LowerLimit.F(print.Time)))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Перерасчет границ канала
        /// </summary>
        private void CalcChannelProperties()
        {
            // Вычисление свойств канала начинаются только кода появится полностью заполненный 2-ой блок.
            // До тех пор, пока в канале меньше 2 блоков, вычисление его свойств бессмысленно,
            // так как линию можно нарисовать минимум по 2-м точкам.
            // Если считать сразу при появлении второго блока то у нас может некорректно определиться тип канала.
            // Например: Имеем первый блок покупок и начавшийся 2-й блок продаж. Вначале мы пометим канал как
            // аптренд (т.к. минимум второго больше), но например цена второго блока падает и если она продолжает падать
            // то мы сначало получим тип канала консолидацию, а пото даунтренд.
            // Поэтому вычислять границы канала и его тип необходимо, когда в в нем присутствуют полностью заполненные 2 блока, а
            // такое может быть когда придет новый принт и он будет отнесен к 3-му блоку. И вычисления для канала должны проходить единожды
            try
            {
                if (Blocks.Count == 3)
                {
                    if (Type == ChannelType.Unknown)
                    {
                        if (Blocks[1].Low > Blocks[2].Low)          // Повышающиеся лои
                            Type = ChannelType.DoubleLowUpTrand;
                        else if (Blocks[1].High > Blocks[2].High)   // Повышающиеся хаи
                            Type = ChannelType.DoubleHighUpTrand;
                        else if (Blocks[1].High < Blocks[2].High)   // Понижающиеся хаи
                            Type = ChannelType.DoubleHighDownTrand;
                        else if (Blocks[1].Low < Blocks[2].Low)     // Понижающиеся лои
                            Type = ChannelType.DoubleLowDownTrand;
                        else if (Blocks[1].High == Blocks[2].High)  // Два хая
                            Type = ChannelType.DoubleHighConsolidation;
                        else if (Blocks[1].Low == Blocks[2].Low)    // Два лоя
                            Type = ChannelType.DoubleLowConsolidation;

                        Impulse firstImpulse = Blocks[2].Impulses[Blocks[2].Impulses.Count - 1];    // Первый импульс первого блока
                        Print firstPrint = firstImpulse.Prints[firstImpulse.Prints.Count - 1];      // Первый принт первого импульса

                        if (Type == ChannelType.DoubleLowUpTrand)
                        {   // Определяем коэффициент наклона
                            LowerLimit.K = (Blocks[1].Low - Blocks[2].Low) / (Blocks[1].Impulses[0].Prints[0].Time.ToBinary() - firstPrint.Time.ToBinary());
                            LowerLimit.A = firstPrint.Time.ToBinary();
                            LowerLimit.B = firstPrint.Price;

                            // Верхняя граница формируется аналогично (параллельно), только со смещением по оси y
                            UpperLimit.K = LowerLimit.K;
                            UpperLimit.A = LowerLimit.A;
                            UpperLimit.B = LowerLimit.B + Blocks[2].Impulses[0].Prints[0].Price;    // Добавляем цену последнего принта первого блока
                        }
                        else if (Type == ChannelType.DoubleHighUpTrand)
                        {
                            UpperLimit.K = (Blocks[1].High - Blocks[2].High) / (Blocks[1].Impulses[0].Prints[0].Time.ToBinary() - firstPrint.Time.ToBinary());
                            UpperLimit.A = firstPrint.Time.ToBinary();
                            UpperLimit.B = firstPrint.Price;

                            LowerLimit.K = UpperLimit.K;
                            LowerLimit.A = UpperLimit.A;
                            LowerLimit.B = UpperLimit.B - Blocks[2].Impulses[0].Prints[0].Price;
                        }
                        else if (Type == ChannelType.DoubleHighDownTrand)
                        {
                            UpperLimit.K = (Blocks[1].High - Blocks[2].High) / (Blocks[1].Impulses[0].Prints[0].Time.ToBinary() - firstPrint.Time.ToBinary());
                            UpperLimit.A = firstPrint.Time.ToBinary();
                            UpperLimit.B = firstPrint.Price;

                            LowerLimit.K = UpperLimit.K;
                            LowerLimit.A = UpperLimit.A;
                            LowerLimit.B = UpperLimit.B - Blocks[2].Impulses[0].Prints[0].Price;    // Отнимаем цену последнего принта первого блока
                        }
                        else if (Type == ChannelType.DoubleLowDownTrand)
                        {
                            LowerLimit.K = (Blocks[1].Low - Blocks[2].Low) / (Blocks[1].Impulses[0].Prints[0].Time.ToBinary() - firstPrint.Time.ToBinary());
                            LowerLimit.A = firstPrint.Time.ToBinary();
                            LowerLimit.B = firstPrint.Price;

                            UpperLimit.K = LowerLimit.K;
                            UpperLimit.A = LowerLimit.A;
                            UpperLimit.B = LowerLimit.B + Blocks[2].Impulses[0].Prints[0].Price;
                        }
                        else if (Type == ChannelType.DoubleHighConsolidation)
                        {
                            UpperLimit.K = 0.0;
                            UpperLimit.A = firstPrint.Time.ToBinary();
                            UpperLimit.B = firstPrint.Price;

                            LowerLimit.K = UpperLimit.K;
                            LowerLimit.A = UpperLimit.A;
                            LowerLimit.B = UpperLimit.B - Blocks[2].Impulses[0].Prints[0].Price;
                        }
                        else if (Type == ChannelType.DoubleLowConsolidation)
                        {
                            LowerLimit.K = 0.0;
                            LowerLimit.A = firstPrint.Time.ToBinary();
                            LowerLimit.B = firstPrint.Price;

                            UpperLimit.K = LowerLimit.K;
                            UpperLimit.A = LowerLimit.A;
                            LowerLimit.B = LowerLimit.B + Blocks[2].Impulses[0].Prints[0].Price;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void Dispose()
        {
            try
            {
                foreach (Block block in _blocks)
                    block.Dispose();

                _blocks.Clear();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
