using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerProtectorCore.Entities;
public class MonitorResult
{
    public int MemoryUsage { get; set; }

    public bool Warning { get; set; }
}
