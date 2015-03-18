using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSAnalisator.Model.PrintStructures;
using System.IO;

namespace TSAnalisator.Model.Analisator.Recorder
{
    public class Recorder : IRecorder
    {
        private string _filename;

        public Recorder()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            _filename = Path.Combine(path, string.Format("{0:dd.MM.yyy}.csv", DateTime.Now));
        }

        public void Write(Channel channel)
        {
            string fullText = string.Format("Channel {0}; {1}; {2};\r\n", 
                                            channel.UpperLimit.K.ToString(),
                                            channel.LowerLimit.K.ToString(),
                                            channel.Type.ToString());

            // Обходим блоки
            for (int i = channel.Blocks.Count - 1; i >= 0; i--)
            {
                fullText += string.Format("\tBlock {0}; {1}; {2}; {3}; {4}; {5};\r\n",
                                          channel.Blocks[i].High.ToString(),
                                          channel.Blocks[i].Low.ToString(),
                                          channel.Blocks[i].Duration.ToString(),
                                          channel.Blocks[i].Speed.ToString(),
                                          channel.Blocks[i].Type.ToString(),
                                          channel.Blocks[i].Volume.ToString());

                // Обходим импульсы
                for (int j = channel.Blocks[i].Impulses.Count - 1; j >= 0; j--)
                {
                    fullText += string.Format("\t\tImpulse {0}; {1}; {2}; {3}; {4}; {5};\r\n",
                                              channel.Blocks[i].Impulses[j].AskVanish.ToString(),
                                              channel.Blocks[i].Impulses[j].BidVanish.ToString(),
                                              channel.Blocks[i].Impulses[j].Duration.ToString(),
                                              channel.Blocks[i].Impulses[j].Price.ToString(),
                                              channel.Blocks[i].Impulses[j].Type.ToString(),
                                              channel.Blocks[i].Impulses[j].Volume.ToString());

                    // Обходим принты
                    for (int k = channel.Blocks[i].Impulses[j].Prints.Count - 1; k >= 0; k--)
                    {
                        fullText += string.Format("\t\t\tPrint {0:dd.MM.yyy HH:mm:ss.fff}; {1}; {2}; {3};\r\n",
                                                  channel.Blocks[i].Impulses[j].Prints[k].Time,
                                                  channel.Blocks[i].Impulses[j].Prints[k].Price.ToString(),
                                                  channel.Blocks[i].Impulses[j].Prints[k].Type.ToString(),
                                                  channel.Blocks[i].Impulses[j].Prints[k].Volume.ToString());
                    }
                }
            }

            File.AppendAllText(_filename, fullText, Encoding.GetEncoding("Windows-1251"));
        }
    }
}
