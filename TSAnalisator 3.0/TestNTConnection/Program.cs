using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace TestNTConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Properties.Settings.Default.DbConnectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("select * from robot.t_channels", connection);

                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                            Console.Write("{0} \t", dr[i]);

                        Console.WriteLine();
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
