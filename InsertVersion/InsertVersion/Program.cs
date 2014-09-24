using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverString = @"Data Source=localhost\SQLEXPRESS;Integrated Security=True";
            string databaseName = @"FTOM";
            string initialCatalog = ";Initial Catalog=" + databaseName;

            string connectionString = serverString + initialCatalog;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE Metadata (SchemaVersion nvarchar(50))";

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Metadata VALUES ('0.1.0')";

                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Success");
            Console.ReadLine();
        }
    }
}
