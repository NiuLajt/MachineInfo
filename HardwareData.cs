using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class HardwareData
    {
        public required CpuDetails Cpu {  get; set; }
        public required RamDetails Ram { get; set; }
        public required GpuDetails Gpu { get; set; }
        public required OsDetails Os { get; set; }
    }
}