using ServerProtectorCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProtectorCore.Monitor
{
    public abstract class IMonitor
    {

        public abstract void SetConfig(MonitorConfig monitorConfig);
        public abstract void StartMonitor(MonitorConfig monitorConfig);
    }
}
