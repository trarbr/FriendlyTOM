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

        internal void Prepare()
        {
            StreamReader reader = new StreamReader(sqlScriptFile);
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

        internal void Execute(SqlConnection con)
        {
            string commandText;
            string[] commands = Regex.Split(allCommands, "^GO", RegexOptions.Multiline);

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            foreach (string command in commands)
            {
                commandText = command.Trim();
                if (!String.IsNullOrWhiteSpace(commandText))
                {
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();

            System.Threading.Thread.Sleep(sleepTime);
        }

    }
}
