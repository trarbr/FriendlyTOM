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

            // copy the install backup into this folder
            string currentDirectory = Directory.GetCurrentDirectory();
            string backupPathSource = currentDirectory + @"\Helpers\install-FTOM-0_1_0.bak";
            string backupPathTarget = backupsFolder + @"\install-FTOM-0_1_0.bak";

            if (!File.Exists(backupPathTarget))
            {
                File.Copy(backupPathSource, backupPathTarget);
            }
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
