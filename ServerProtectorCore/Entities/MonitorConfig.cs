using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProtectorCore.Entities
{
    public class MonitorConfig
    {
        /// <summary>
        /// 目标内存
        /// </summary>
        public int MemoryTarget { get; set; }

        public string? ServerPath { get; set; }

        public bool AutoReboot { get; set; }

        public int RebootSeconds { get; set; }

        public string RconHost { get; set; }

        public int RconPort { get; set; }

        public string RconPassword { get; set; }


        public bool ProcessWatch { get; set; }

        public string Args { get; set; }

        public int Interval { get; set; }

        public string BackupPath { get; set; }

        public string GameDataPath { get; set; }

    }
}
