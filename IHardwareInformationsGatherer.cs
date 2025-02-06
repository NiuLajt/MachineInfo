using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal interface IHardwareInformationsGatherer
    {
        HardwareData GetHardwareData();
        CpuDetails GetCpuDetails();
        RamDetails GetRamDetails();
        GpuDetails GetGpuDetails();
        OsDetails GetOsDetails();
    }
}