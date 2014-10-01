using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Domain.Controller
{
    public class SettingsController
    {
        private IDataAccessFacade dataAccessFacade;
        private string backupsFolder;
        private string friendlyTOMFolder;
        private const string VERSION = "0.1.0";

        public SettingsController()
        {
            this.dataAccessFacade = DataAccessFacade.GetInstance();

            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            friendlyTOMFolder = Path.Combine(myDocuments, "FriendlyTOM");
            backupsFolder = Path.Combine(friendlyTOMFolder, "Backups");
        }

        public void FirstRunSetup()
        {
            createFolder(friendlyTOMFolder);
            createFolder(backupsFolder);

            setupBackupPermissions(backupsFolder);

            // run the database setup 
            setupDatabase();
        }

        private void setupDatabase()
        {
            /*
             * installer copies sqlscripts into INSTALLFOLDER/SqlScripts
             * SettingsController calls DAF.SetupDatabase(versionnumber)
             * DAF forwards call to SqlManager
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

            /*
             * sqlscripts are named like: install_$version_001.sql where install can also be migrate
             * scripts must be fired in order, 001 first, 002 second etc
             * only automaticly script creation of tables and stored procedures, remember to get schema and data!
             * and put parammeterized sql text in the files (for later versions)
             */

            /*
             * sqlscript formats:
             * header first line: sleeptime
             * rest of header: comment explaning what is happening
             * blank line seperates header from body
             * body contains sql statements, seperated by go statements
             */

            dataAccessFacade.SetupDatabase(VERSION);
            dataAccessFacade.InitializeDatabase();
        }

        public void BackupDatabase()
        {
            //string date = DateTime.Today.ToShortDateString();
            string backupName = String.Format("{0:yyyy-MM-dd_HHmmss}.bak", DateTime.Now);
            string backupPath = Path.Combine(backupsFolder, backupName);
            dataAccessFacade.BackupDatabase(backupPath);
        }

        public void RestoreDatabase(string timestamp)
        {
            string backupName = timestamp + ".bak";
            string backupPath = Path.Combine(backupsFolder, backupName);
            dataAccessFacade.RestoreDatabase(backupPath);
        }

        private void setupBackupPermissions(string backupsFolder)
        {
            bool sqlUserHasPermission = false;
            string sqlUserName = @"NT SERVICE\MSSQL$SQLEXPRESS";

            DirectoryInfo dirInfo = new DirectoryInfo(backupsFolder);
            DirectorySecurity dirSec = dirInfo.GetAccessControl();
            AuthorizationRuleCollection rules = dirSec.GetAccessRules(true, true, typeof(NTAccount));

            // iterate over all rules, check if any of them has sqluser and read write permission
            // if not, create the rules and add it
            int ruleIndex = 0;
            int ruleCount = rules.Count;
            while (!sqlUserHasPermission && ruleIndex < ruleCount)
            {
                FileSystemAccessRule rule = (FileSystemAccessRule)rules[ruleIndex];
                if (rule.IdentityReference.Value.Equals(sqlUserName))
                {
                    // check if it has read and write permissions
                    if (rule.FileSystemRights.HasFlag(FileSystemRights.Write) && 
                        rule.FileSystemRights.HasFlag(FileSystemRights.Read))
                    {
                        sqlUserHasPermission = true;
                    }
                }

                ruleIndex++;
            }

            if (!sqlUserHasPermission)
            {
                FileSystemAccessRule readAccess = new FileSystemAccessRule(
                    sqlUserName,
                    FileSystemRights.Read,
                    InheritanceFlags.ObjectInherit,
                    PropagationFlags.InheritOnly,
                    AccessControlType.Allow);
                FileSystemAccessRule writeAccess = new FileSystemAccessRule(
                    sqlUserName,
                    FileSystemRights.Write,
                    AccessControlType.Allow);

                dirSec.AddAccessRule(readAccess);
                dirSec.AddAccessRule(writeAccess);
                dirInfo.SetAccessControl(dirSec);
            }
        }

        private string createFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            return folderName;
        }

        public List<string> GetListOfBackups()
        {
            List<string> backups = new List<string>();
            try
            {
                string[] filenames = Directory.GetFiles(backupsFolder);
                foreach (string filename in filenames)
                {
                    string backup = Path.GetFileNameWithoutExtension(filename);
                    backups.Add(backup);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return backups;
        }
    }
}
