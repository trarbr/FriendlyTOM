﻿/*
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
        #region Setup
        internal SettingsController(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;

            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            friendlyTOMFolder = Path.Combine(myDocuments, "FriendlyTOM");
            backupsFolder = Path.Combine(friendlyTOMFolder, "Backups");
        }

        internal void FirstRunSetup()
        {
            ensureFolderExists(friendlyTOMFolder);
            ensureFolderExists(backupsFolder);

            ensureSqlBackupPermissions(backupsFolder);

            // run the database setup 
            setupDatabase();
        }

        private string ensureFolderExists(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            return folderName;
        }
        private void ensureSqlBackupPermissions(string backupsFolder)
        {
            bool sqlUserHasPermission = false;
            string sqlUserName = @"NT SERVICE\MSSQL$SQLEXPRESS";

            DirectoryInfo dirInfo = new DirectoryInfo(backupsFolder);
            DirectorySecurity dirSec = dirInfo.GetAccessControl();
            AuthorizationRuleCollection rules = dirSec.GetAccessRules(true, true, typeof(NTAccount));

            // iterate over all rules, check if any of them has sqluser and read write permission
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

            // if no permssions for sqluser found, create the rules and add them
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
        private void setupDatabase()
        {
            dataAccessFacade.SetupDatabase(version);
            dataAccessFacade.InitializeDatabase();
        }
        #endregion

        public void BackupDatabase()
        {
            string backupName = String.Format("{0:yyyy-MM-dd_HHmmss}-{1}.bak", DateTime.Now, 
                version);
            string backupPath = Path.Combine(backupsFolder, backupName);
            dataAccessFacade.BackupDatabase(backupPath);
        }

        public void RestoreDatabase(string backup)
        {
            string backupName = backup + ".bak";
            string backupPath = Path.Combine(backupsFolder, backupName);
            dataAccessFacade.RestoreDatabase(backupPath);
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

        private IDataAccessFacade dataAccessFacade;
        private string backupsFolder;
        private string friendlyTOMFolder;
        private readonly Version version = Version.Parse("0.2.0");
    }
}
