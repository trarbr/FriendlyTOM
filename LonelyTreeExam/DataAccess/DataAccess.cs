using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataAccess
    {
        #region OpenSQLConnection
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
            }
        }
        /// <summary>
        /// This method is supposed to read the connection string from a text file.
        /// and return it for the open connection method.
        /// </summary>
        /// <returns>connString</returns>
        static private string GetConnectionString()
        {
            string connString = File.ReadAllText("C:\\ConnectString.txt");
            return connString;
        }
        #endregion

        #region CRUD
        private static void ReadAll()
        {
            
        }

        private static void Delete()
        {
            
        }

        private static void Insert()
        {
            
        }

        private static void Update()
        {
            
        }
        #endregion 
    }
}
