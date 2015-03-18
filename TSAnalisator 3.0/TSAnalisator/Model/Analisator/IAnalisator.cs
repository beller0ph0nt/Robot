using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TSAnalisator.Model.PrintStructures;

namespace TSAnalisator.Model.Analisator
{
    public interface IAnalisator
    {
        void Add(Print print);
    }
}
