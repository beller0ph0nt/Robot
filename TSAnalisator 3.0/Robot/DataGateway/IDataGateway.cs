using System;

namespace Robot.DataGateway
{
    public interface IDataGateway
    {
        void Write(Object package);
        void Read(Object package);
    }
}
