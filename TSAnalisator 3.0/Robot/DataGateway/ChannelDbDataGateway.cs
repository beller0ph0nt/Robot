using Npgsql;
using Robot.TransportStructures;
using System;
using System.Collections.Generic;
using System.Globalization;
using Debugging;

namespace Robot.DataGateway
{
    public class ChannelDbDataGateway : IDataGateway
    {
        private readonly string _connectionString;
        private readonly CultureInfo _culture;

        public ChannelDbDataGateway()
            : this("postgres", "postgres", "q1w2e3r4Z", "127.0.0.1", "5432")
        {
        }

        public ChannelDbDataGateway(string DataBase, string UserId, string Password, string Server, string Port)
        {
            _connectionString = "PORT=" + Port + ";" +
                    "TIMEOUT=15;" +
                    "POOLING=True;" +
                    "MINPOOLSIZE=1;" +
                    "MAXPOOLSIZE=20;" +
                    "COMMANDTIMEOUT=20;" +
                    "COMPATIBLE=2.2.4.3;" +
                    "DATABASE=" + DataBase + ";" +
                    "SERVER=" + Server + ";" +
                    "PASSWORD=" + Password + ";" +
                    "USER ID=" + UserId;

            _culture = new CultureInfo("en-US");
        }

        public void Write(object package)
        {
            Log.Write("ChannelDbDataGateway.Write Begin");

            if (package is Channel)
            {
                IChannel channel = package as Channel;

                Log.Write("Creating sql connection...");
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    Log.Write("Creating sql command...");
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "insert into robot.t_channels(r_type) values (" + (int)channel.Type + ");";

                        IBlock block;
                        List<IBlock> blocks = channel.Blocks;

                        for (int i = blocks.Count - 1; i >= 0; i--)
                        {
                            block = blocks[i];

                            command.CommandText += "insert into robot.t_blocks" +
                                "(r_high_x, r_high_y, r_low_x, r_low_y, r_duration, r_distance, r_volume, r_type, r_channel_id)" +
                                "values (" + block.High.X.ToString("F", _culture) + ", " +
                                             block.High.Y.ToString("F", _culture) + ", " +
                                             block.Low.X.ToString("F", _culture) + ", " +
                                             block.Low.Y.ToString("F", _culture) + ", " +
                                             block.Duration.Ticks + ", " +
                                             block.Distance.ToString("F", _culture) + ", " +
                                             block.Volume + ", " +
                                             (int)block.Type + ", " +
                                             "currval('robot.t_channels_r_channel_id_seq'));";

                            IImpulse impulse;
                            List<IImpulse> impulses = block.Impulses;

                            for (int j = impulses.Count - 1; j >= 0; j--)
                            {
                                impulse = impulses[j];

                                command.CommandText += "insert into robot.t_impulses" +
                                    "(r_bid_vanish, r_ask_vanish, r_high_x, r_high_y, r_low_x, r_low_y, r_duration, r_price, r_volume, r_type, r_block_id)" +
                                    "values (" + impulse.BidVanish + ", " +
                                                 impulse.AskVanish + ", " +
                                                 impulse.High.X.ToString("F", _culture) + ", " +
                                                 impulse.High.Y.ToString("F", _culture) + ", " +
                                                 impulse.Low.X.ToString("F", _culture) + ", " +
                                                 impulse.Low.Y.ToString("F", _culture) + ", " +
                                                 impulse.Duration.Ticks + ", " +
                                                 impulse.Price.ToString("F", _culture) + ", " +
                                                 impulse.Volume + ", " +
                                                 (int)impulse.Type + ", " +
                                                 "currval('robot.t_blocks_r_block_id_seq'));";

                                IPrint print;
                                List<IPrint> prints = impulse.Prints;

                                for (int k = prints.Count - 1; k >= 0; k--)
                                {
                                    print = prints[k];

                                    command.CommandText += "insert into robot.t_prints" +
                                        "(r_price, r_type, r_time, r_volume, r_day_of_year, r_month, r_day, r_day_of_week, r_hour, r_impulse_id)" +
                                        "values (" + print.Price.ToString("F", _culture) + ", " +
                                                     (int)print.Type + ", " +
                                                     print.Time.Ticks + ", " +
                                                     print.Volume + ", " +
                                                     print.Time.DayOfYear + ", " +
                                                     print.Time.Month + ", " +
                                                     print.Time.Day + ", " +
                                                     (int)print.Time.DayOfWeek + ", " +
                                                     print.Time.Hour + ", " +
                                                     "currval('robot.t_impulses_r_impulse_id_seq'));";
                                }
                            }
                        }

                        connection.Open();

                        Log.Write("Execution sql query...");
                        command.ExecuteNonQuery();
                        Log.Write("Sql query executed...");
                    }
                }
            }
            else
                throw new Exception("Неизвестный тип данных");

            Log.Write("ChannelDbDataGateway.Write End");
        }

        public void Read(object package)
        {
            throw new NotImplementedException();
        }

        public void GetTrainingSet()
        {
            throw new NotImplementedException();
        }
    }
}
