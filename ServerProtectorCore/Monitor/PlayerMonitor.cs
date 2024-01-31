using ServerProtectorCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServerProtectorCore.Monitor
{
    public class PlayerMonitor : IMonitor
    {
        private MonitorConfig monitorConfig;

        private System.Timers.Timer checkTimer;

        private static PlayerMonitor _instance;

       

        public static PlayerMonitor Initialize()
        {
            if (null == _instance)
            {
                _instance = new PlayerMonitor();

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
            try
            {
                if (!monitorConfig.CheckPlayers)
                {
                    return;
                }

                RconUtils.TestConnection(monitorConfig.RconHost, monitorConfig.RconPort, monitorConfig.RconPassword);
                var players = RconUtils.ShowPlayers();

                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
