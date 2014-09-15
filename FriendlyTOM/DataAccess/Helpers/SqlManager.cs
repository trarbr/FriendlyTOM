/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    class SqlManager
    {
        public string ConnectionString { get; private set; }
        private string serverString;
        private string databaseName;
        private string initialCatalog;

        public SqlManager(string serverString, string databaseName)
        {
            // TODO: Remove initialCatalog
            this.serverString = serverString;
            this.databaseName = databaseName;
            initialCatalog = ";Initial Catalog=" + databaseName;
            ConnectionString = serverString + initialCatalog;
        }

        public void SetupDatabase()
        {
            // try and establish connection
            if (!databaseExists())
            {
                // if it fails, run setup script
                runSetup();
            }
        }

        public void BackupDatabase(string backupPath)
        {
            // backupPath must have trailing backslash
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "BackupDatabase";

                    SqlParameter parameter = new SqlParameter("@BackupPath", backupPath);
                    cmd.Parameters.Add(parameter);

                    con.Open();
                    // make sure to catch and report errors!
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RestoreDatabase(string backupPath)
        {
            using (SqlConnection con = new SqlConnection(serverString + ";Initial Catalog=" + "master"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RestoreDatabase";

                    SqlParameter parameter = new SqlParameter("@BackupPath", backupPath);
                    cmd.Parameters.Add(parameter);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool databaseExists()
        {
            SqlConnection con = new SqlConnection(serverString + initialCatalog);
            try
            {
                con.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        private void runSetup()
        {
            // create and configure the database
            using (SqlConnection con = new SqlConnection(serverString))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                string create_command = String.Format("CREATE DATABASE {0}", databaseName);

                cmd.CommandText = create_command;

                con.Open();
                cmd.ExecuteNonQuery();

                // configure database
                List<string> commands = getCommands(@"create-settings.sql");
                foreach (string command in commands)
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
            }

            // wait to make sure the new database is ready to accept connections
            System.Threading.Thread.Sleep(10000);

            // create tables and stored procedures
            using (SqlConnection con = new SqlConnection(serverString + initialCatalog))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();

                List<string> commands = getCommands(@"create-schema.sql");

                foreach (string command in commands)
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private List<string> getCommands(string filename)
        {
            List<string> commands = new List<string>();

            string sql_file = File.ReadAllText(filename);
            string[] dirt_commands = Regex.Split(sql_file, "^GO", RegexOptions.Multiline);
            foreach (string dirty_command in dirt_commands)
            {
                String command = dirty_command.Trim();
                if (!String.IsNullOrWhiteSpace(command))
                {
                    commands.Add(command);
                }
            }

            return commands;
        }
    }
}
