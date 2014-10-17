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

        public void SetupDatabase(Version version)
        {
            /*
             * SqlManager tries to connect to DB.
             *      If it fails
             *          it gets all the install_$version scripts where $version = versionnumber
             *      If it succeeds
             *          it checks that the SchemaVersion is equal to what settingsController set as versionnumber
             *              If not
             *                  if gets all migrate_ scripts with SchemaVersion < $version <= versionnumber
             *              if yes:
             *                  it gets nothing
             *                  
             * SqlManager executes all the scripts
            */

            // try and establish connection
            if (!databaseExists())
            {
                // if it fails, create the database
                createDatabase(version);
            }
            // now check version number
            Version schemaVersion = getSchemaVersion();
            if (schemaVersion != version)
            {
                upgradeSchema(schemaVersion, version);
            }
        }

        private void upgradeSchema(Version fromVersion, Version toVersion)
        {
            // get the list of all migrations newer than fromVersion
            List<Version> next_versions = new List<Version>();
            string[] migrate_scripts = Directory.GetFiles(@"SqlScripts\", "migrate_*_000.sql");
            foreach (string script in migrate_scripts)
            {
                string versionAsString = script.Split('_')[1];
                versionAsString = versionAsString.Replace('-', '.');
                Version version = new Version(versionAsString);
                if (version > fromVersion)
                {
                    next_versions.Add(version);
                }
            }
            next_versions.Sort();

            int i = 0;
            while (fromVersion != toVersion && i < next_versions.Count)
            {
                Version nextVersion = next_versions[i];
                migrateDatabase(nextVersion);
                fromVersion = getSchemaVersion();
                i++;
            }

        }

        private void migrateDatabase(Version nextVersion)
        {
            // find the scripts
            string versionWithDashes = nextVersion.ToString().Replace('.', '-');
            string[] sqlScriptFiles = Directory.GetFiles(@"SqlScripts\", 
                String.Format("migrate_{0}_*.sql", versionWithDashes));

            // initialize scripts
            List<SqlScript> sqlScripts = new List<SqlScript>();
            foreach (string sqlScriptFile in sqlScriptFiles)
            {
                SqlScript sqlScript = new SqlScript(sqlScriptFile);
                sqlScript.ReadCommands();
                sqlScripts.Add(sqlScript);
            }

            // run the scripts
            using (SqlConnection con = new SqlConnection(serverString))
            {
                foreach (SqlScript sqlScript in sqlScripts)
                {
                    sqlScript.Execute(con);
                }
            }
        }

        private Version getSchemaVersion()
        {
            Version schemaVersion = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT SchemaVersion FROM Metadata";

                    con.Open();
                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Read();

                    string schemaVersionAsString = (string)reader["SchemaVersion"];

                    schemaVersion = new Version(schemaVersionAsString);
                }
            }

            return schemaVersion;
        }

        private void createDatabase(Version version)
        {
            /*
             * sqlscripts are named like: install_$version_000.sql where install can also be migrate
             * scripts must be fired in order, 000 first, 001 second etc
             * only automaticly script creation of tables and stored procedures, remember to get schema and data!
             * and put parammeterized sql text in the files (for later versions)
             */

            string versionWithDashes = version.ToString().Replace('.', '-');
            // get all files with name install_version_*
            string[] sqlScriptFiles = Directory.GetFiles(@"SqlScripts\", 
                String.Format("install_{0}_*.sql", versionWithDashes));

            // execute the scripts
            List<SqlScript> sqlScripts = new List<SqlScript>();

            foreach (string sqlScriptFile in sqlScriptFiles)
            {
                SqlScript sqlScript = new SqlScript(sqlScriptFile);
                sqlScript.ReadCommands();
                sqlScripts.Add(sqlScript);
            }

            using (SqlConnection con = new SqlConnection(serverString))
            {
                foreach (SqlScript sqlScript in sqlScripts)
                {
                    sqlScript.Execute(con);
                }
            }
        }

        public void BackupDatabase(string backupPath)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "BACKUP DATABASE FTOM TO DISK = @backupPath";
                    cmd.Parameters.AddWithValue("backupPath", backupPath);

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        throw new ArgumentOutOfRangeException("backupPath",
                            "Backup failed! The database probably doesn't have write permissions to the selected folder!");
                    }
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

                cmd = con.CreateCommand();
                cmd.CommandText = "RESTORE DATABASE FTOM FROM DISK = @backupPath";

                cmd.Parameters.AddWithValue("backupPath", backupPath);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw new ArgumentOutOfRangeException("backupPath",
                        "Restore failed because of error. Probably the selected backup file is corrupt!");
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
    }
}
