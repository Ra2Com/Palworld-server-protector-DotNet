using ServerProtectorCore.Entities;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServerProtectorCore.Monitor
{
    public class SaveMonitor : IMonitor
    {
        private static SaveMonitor _instance;

        private MonitorConfig monitorConfig;

        private System.Timers.Timer checkTimer;
        public static SaveMonitor Initialize()
        {
            if (null == _instance)
            {
                _instance = new SaveMonitor();

                return _instance;
            }

            return _instance;
        }

        public override void SetConfig(MonitorConfig monitorConfig)
        {
            this.monitorConfig = monitorConfig;
        }

        public override void StartMonitor(MonitorConfig monitorConfig)
        {
            SetConfig(monitorConfig);
            SetConfig(monitorConfig);

            checkTimer = new System.Timers.Timer
            {

                Interval = monitorConfig.Interval * 1000,
                Enabled = true
            };
            checkTimer.Elapsed += CheckTimer_Elapsed;
            checkTimer.Start();
        }

        private void CheckTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            CopyGameDataToBackupPath();
        }

        private void CopyGameDataToBackupPath()
        {
            try
            {
                var backupPath = monitorConfig.BackupPath;
                var gamedataPath = monitorConfig.GameDataPath;
                if (string.IsNullOrEmpty(backupPath))
                {
                   
                    return;
                }
                string backupFolderName = $"SaveGames-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.zip";
                string backupFilePath = Path.Combine(backupPath, backupFolderName);

                if (!Directory.Exists(gamedataPath))
                {
                    
                    return;
                }

                if (!Directory.Exists(backupPath))
                {
                    
                    return;
                }

                string tempGameDataPath = Path.Combine(Path.GetTempPath(), "TempGameData");
                Directory.CreateDirectory(tempGameDataPath);
                string tempGameDataCopyPath = Path.Combine(tempGameDataPath, "GameData");

                
                DirectoryCopy(gamedataPath, tempGameDataCopyPath, true);

                
                ZipFile.CreateFromDirectory(tempGameDataCopyPath, backupFilePath);

                
                Directory.Delete(tempGameDataPath, true);

               
            }
            catch (Exception ex)
            {
               
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception
            if (!dir.Exists)
            {
                return;
            }

            // If the destination directory does not exist, create it
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
