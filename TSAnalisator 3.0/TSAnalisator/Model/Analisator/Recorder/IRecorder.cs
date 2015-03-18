using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSAnalisator.Model.PrintStructures;

namespace TSAnalisator.Model.Analisator.Recorder
{
    public interface IRecorder
    {
        void Write(Channel channel);
    }
}
