using Hardware.Info;
using ServerProtectorCore.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProtectorCore.Monitor
{
    public class MemoryMonitor : IMonitor
    {
        private HardwareInfo hardwareInfo;
        private static MemoryMonitor _instance;

        private MonitorConfig monitorConfig;

        public Action<MonitorResult> ResultAction;

        private System.Timers.Timer checkTimer;
        public static MemoryMonitor Initialize()
        {
            if (null == _instance)
            {
                _instance = new MemoryMonitor();

                return _instance;
            }

            return _instance;
        }
        private MemoryMonitor()
        {
            hardwareInfo = new HardwareInfo();
        }
        public override void SetConfig(MonitorConfig monitorConfig)
        {

            this.monitorConfig = monitorConfig;
        }

        public override void StartMonitor(MonitorConfig monitorConfig)
        {

            SetConfig(monitorConfig);

            checkTimer = new System.Timers.Timer
            {

                Interval = monitorConfig.Interval * 1000,
                Enabled = true
            };
            checkTimer.Elapsed += CheckTimer_Elapsed;
            checkTimer.Start();
        }
        private void Start()
        {

            var monitorResult = new MonitorResult();
            var memoryUsage = Math.Round(GetSystemMemoryUsagePercentage(), 2);

            var serverPath = monitorConfig.ServerPath;
            if (monitorConfig.AutoReboot)
            {
                try
                {
                    var isProcessRunning = IsProcessRunning(serverPath);

                    if (memoryUsage >= monitorConfig.MemoryTarget)
                    {

                        monitorResult.Warning = isProcessRunning;

                    }

                    if (isProcessRunning)
                    {

                        // 使用rcon向服务端发送指令
                        RconUtils.TestConnection(monitorConfig.RconHost, monitorConfig.RconPort, monitorConfig.RconPassword);
                        var info = RconUtils.SendMsg("save");
                        var rebootSeconds = monitorConfig.RebootSeconds;
                        var result = RconUtils.SendMsg($"Shutdown {rebootSeconds} The_server_will_restart_in_{rebootSeconds}_seconds.");

                    }

                }
                catch (Exception ex)
                {


                }
            }
            if (monitorConfig.ProcessWatch)
            {
                var isProcessRunning = IsProcessRunning(serverPath);
                if (!isProcessRunning)
                {
                    try
                    {

                        var args = monitorConfig.Args;
                        if (string.IsNullOrEmpty(serverPath))
                        {
                            return;
                        }
                        if (!string.IsNullOrEmpty(args))
                        {
                            Process.Start(serverPath, args);
                        }
                        else
                        {
                            Process.Start(serverPath);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        private void CheckTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Start();
        }

        private float GetSystemMemoryUsagePercentage()
        {
            hardwareInfo.RefreshMemoryStatus();
            var info = hardwareInfo.MemoryStatus;

            var memoryUsage = (info.TotalPhysical - info.AvailablePhysical) / info.TotalPhysical;

            return (float)(memoryUsage * 100);
        }

        private bool IsProcessRunning(string? processPath)
        {
            if (string.IsNullOrEmpty(processPath))
            {
                return false;
            }
            var processName = Path.GetFileNameWithoutExtension(processPath);
            var processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }
    }
}
