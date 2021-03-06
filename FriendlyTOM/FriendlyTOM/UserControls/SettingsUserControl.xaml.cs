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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Domain.Controller;
using Microsoft.Win32;

namespace FriendlyTOM.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        private SettingsController settingsController;
        public SettingsUserControl(SettingsController settingsController)
        {
            InitializeComponent();

            this.settingsController = settingsController;
        }

        private void hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void backupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                settingsController.BackupDatabase();
                refreshBackupsListBox();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedBackup = backupsListBox.SelectedItem.ToString();
            try
            {
                settingsController.RestoreDatabase(selectedBackup);
                MessageBox.Show("Database restored successfully! Please restart Friendly TOM to load changes.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshBackupsListBox()
        {
            // load list of backups, put in listbox
            List<string> backups = settingsController.GetListOfBackups();

            backupsListBox.ItemsSource = null;
            backupsListBox.ItemsSource = backups;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            refreshBackupsListBox();
        }
    }
}
