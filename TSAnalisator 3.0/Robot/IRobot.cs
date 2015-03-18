using System;

namespace Robot
{
    public interface IRobot
    {
        void Learn();
        void Save(Object obj);
        void Work(Object obj);
    }
}
