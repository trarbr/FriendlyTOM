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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    class SqlScript
    {
        private string sqlScriptFile;
        private string allCommands;
        private int sleepTime;

        public SqlScript(string sqlScriptFile)
        {
            this.sqlScriptFile = sqlScriptFile;
        }

        internal void ReadCommands()
        {
            /*
             * sqlscript format:
             * header first line: sleeptime
             * rest of header: comment explaning what is happening
             * blank line seperates header from body
             * body contains sql statements, seperated by go statements
             */
            using (StreamReader reader = new StreamReader(sqlScriptFile))
            {
                List<string> header = new List<string>();

                string line = reader.ReadLine();
                while (line != "")
                {
                    header.Add(line);
                    line = reader.ReadLine();
                }
                sleepTime = int.Parse(header[0].Split(':')[1].Trim());

                allCommands = reader.ReadToEnd();
            }
        }

        internal void Execute(SqlConnection con)
        {
            string commandText;
            string[] commands = Regex.Split(allCommands, "^GO", RegexOptions.Multiline);

            con.Open();
            SqlCommand cmd;
            foreach (string command in commands)
            {
                commandText = command.Trim();
                if (!String.IsNullOrWhiteSpace(commandText))
                {
                    cmd = con.CreateCommand();
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();

            System.Threading.Thread.Sleep(sleepTime);
        }

    }
}
