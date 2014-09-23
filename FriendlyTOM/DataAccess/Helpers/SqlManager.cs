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
using System.Threading;
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
                // if it fails, restore install backup
                // TODO: Fix the initialization stuff!
                string backupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                backupPath += @"\FriendlyTOM\Backups\install-FTOM-0_1_0.bak";
                RestoreDatabase(backupPath);
                Thread.Sleep(10000);
            }
        }

        public void BackupDatabase(string backupPath)
        {
            backupPath += DateTime.Today.ToShortDateString() + ".bak";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "BACKUP DATABASE FTOM TO DISK = @backupPath";
                    cmd.Parameters.AddWithValue("backupPath", backupPath);

                    con.Open();
                    //try
                    //{
                        cmd.ExecuteNonQuery();
                    //}
                    //catch (SqlException)
                    //{
                    //    throw new ArgumentOutOfRangeException("backupPath",
                    //        "Backup failed! The database probably doesn't have write permissions to the selected folder!");
                    //}
                }
            }
        }

        public void RestoreDatabase(string backupPath)
        {
            using (SqlConnection con = new SqlConnection(serverString + ";Initial Catalog=" + "master"))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // first kick all users 
                cmd.CommandText = @"
                    DECLARE @spid varchar(10)
                    SELECT @spid = spid
                      FROM master.sys.sysprocesses
    				  WHERE dbid = DB_ID('FTOM')
    			    WHILE @@ROWCOUNT <> 0
                    BEGIN
                      EXEC('KILL ' + @spid)
                      SELECT @spid = spid
                        FROM master.sys.sysprocesses
                        WHERE
                          dbid = DB_ID('FTOM') AND
                          spid > @spid
                    END";
                con.Open();
                cmd.ExecuteNonQuery();

                // drop the database??
                    // DROP DATABASE FTOM";

                // then restore the database
                cmd = con.CreateCommand();
                cmd.CommandText = "RESTORE DATABASE FTOM FROM DISK = @backupPath";

                cmd.Parameters.AddWithValue("backupPath", backupPath);

                //try
                //{
                    cmd.ExecuteNonQuery();
                //}
                //catch (SqlException)
                //{
                //    throw new ArgumentOutOfRangeException("backupPath", 
                //        "Restore failed because of error. Probably the selected backup file is corrupt!");
                //}
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
    }
}
