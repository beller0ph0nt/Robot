using System.Collections.Generic;
using TSAnalisator.Model.PrintStructures;

namespace TSAnalisator.View
{
    public interface IView
    {
        void Draw(List<DrawPrint> prints);
        void Draw(long totalVolume);
    }
}
