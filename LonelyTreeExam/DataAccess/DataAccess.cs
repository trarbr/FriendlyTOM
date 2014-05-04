using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataAccess
    {
        /// <summary>
        /// This method is for connection to the database.
        /// </summary>
        private static void OpenSqlConnection()
        {
            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;

                connection.Open();

                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("ConnectionString: {0}",
                    connection.ConnectionString);
            }
        }
        /// <summary>
        /// This method is supposed to read the connection string from a text file.
        /// and return it for the open connection method.
        /// </summary>
        /// <returns></returns>
        static private string GetConnectionString()
        {
            const string connString = @"C:\Users\Spaak\Documents\GitHub\LonelyTreeExam\ConnectString.txt";
            return connString;
        }
    }
}
