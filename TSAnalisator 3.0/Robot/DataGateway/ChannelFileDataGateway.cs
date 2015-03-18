using System;
using System.IO;
using System.Text;
using Robot.TransportStructures;

namespace Robot.DataGateway
{
    public class ChannelFileDataGateway : IDataGateway
    {
        private string _filename;

        public ChannelFileDataGateway()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _filename = Path.Combine(path, string.Format("{0:dd.MM.yyy}.csv", DateTime.Now));
        }

        public void Write(Object package)
        {
            try
            {
                if (package is Channel)
                {
                    IChannel channel = package as Channel;

                    string fullText = string.Format("{0}\r\n", channel.ToString());

                    // Обходим блоки
                    for (int i = channel.Blocks.Count - 1; i >= 0; i--)
                    {
                        fullText += string.Format("\t{0}\r\n", channel.Blocks[i].ToString());

                        // Обходим импульсы
                        for (int j = channel.Blocks[i].Impulses.Count - 1; j >= 0; j--)
                        {
                            fullText += string.Format("\t\t{0}\r\n", channel.Blocks[i].Impulses[j].ToString());

                            // Обходим принты
                            for (int k = channel.Blocks[i].Impulses[j].Prints.Count - 1; k >= 0; k--)
                                fullText += string.Format("\t\t\t{0}\r\n", channel.Blocks[i].Impulses[j].Prints[k].ToString());
                        }
                    }

                    File.AppendAllText(_filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
                else
                    throw new Exception("Неизвестный тип данных");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Read(Object package)
        {
        }
    }
}
