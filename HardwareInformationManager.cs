using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class HardwareInformationManager
    {
        IHardwareInformationsGatherer _gatherer;

        public HardwareInformationManager(IHardwareInformationsGatherer gatherer)
        {
            _gatherer = gatherer;
        }

        public CpuDetails GetCpuDetails() => _gatherer.GetCpuDetails();
        public GpuDetails GetGpuDetails() => _gatherer.GetGpuDetails();
        public RamDetails GetRamDetails() => _gatherer.GetRamDetails();
        public OsDetails GetOsDetails() => _gatherer.GetOsDetails();

        public string GetCpuString()
        {
            throw new NotImplementedException();
        }

        public string GetGpuString()
        {
            throw new NotImplementedException();
        }

        public string GetOsString()
        {
            throw new NotImplementedException();
        }

        public string GetRamString()
        {
            throw new NotImplementedException();
        }
    }
}
