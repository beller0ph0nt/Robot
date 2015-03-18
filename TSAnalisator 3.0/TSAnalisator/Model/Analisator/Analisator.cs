using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TSAnalisator.Model.PrintStructures;
using TSAnalisator.Model.Analisator.Recorder;
using Debugging;

namespace TSAnalisator.Model.Analisator
{
    public class Analisator : IAnalisator, IDisposable
    {
        private List<Channel> _channels;
        private int _maxChannelCount;
        private IRecorder _recorder;

        public Analisator(int maxChannelCount, IRecorder recorder)
        {
            _maxChannelCount = maxChannelCount;
            _recorder = recorder;
            _channels = new List<Channel>();
        }

        public void Add(Print print)
        {
            try
            {
                if (_channels.Count > 0)
                {
                    if (_channels[0].IsPrintInChannel(print) == false)
                    {
                        _channels.Insert(0, new Channel());

                        if (_recorder != null)
                            _recorder.Write(_channels[1]);
                    }
                }
                else
                    _channels.Insert(0, new Channel());

                // Вставить блок удаляющий канал, если длинна списка превысила _maxChannelCount
                _channels[0].Add(print);

                if (_channels.Count > _maxChannelCount)
                {
                    _channels[_maxChannelCount].Dispose();
                    _channels.RemoveAt(_maxChannelCount);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void Dispose()
        {
            try
            {
                foreach (Channel channel in _channels)
                    channel.Dispose();

                _channels.Clear();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
