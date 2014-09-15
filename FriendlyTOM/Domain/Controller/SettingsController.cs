using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;

namespace Domain.Controller
{
    public class SettingsController
    {
        private IDataAccessFacade dataAccessFacade;
        public SettingsController()
        {
            this.dataAccessFacade = DataAccessFacade.GetInstance();
        }

        public void BackupDatabase(string backupPath)
        {
            dataAccessFacade.BackupDatabase(backupPath);
        }

        public void RestoreDatabase(string backupPath)
        {
            dataAccessFacade.RestoreDatabase(backupPath);
        }

    }
}
