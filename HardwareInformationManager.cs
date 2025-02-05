using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class HardwareInformationManager
    {
        readonly IHardwareInformationsGatherer _gatherer;

        public HardwareInformationManager(IHardwareInformationsGatherer gatherer)
        {
            _gatherer = gatherer;
        }

        public CpuDetails GetCpuDetails() => _gatherer.GetCpuDetails();
        public RamDetails GetRamDetails() => _gatherer.GetRamDetails();
        public GpuDetails GetGpuDetails() => _gatherer.GetGpuDetails();
        public OsDetails GetOsDetails() => _gatherer.GetOsDetails();

        public string GetCpuSummary()
        {
            throw new NotImplementedException();
        }
        public string GetCpuDescription()
        {
            throw new NotImplementedException();
        }

        public string GetRamSummary()
        {
            throw new NotImplementedException();
        }
        public string GetRamDescription()
        {
            throw new NotImplementedException();
        }

        public string GetGpuSummary()
        {
            throw new NotImplementedException();
        }
        public string GetGpuDescription()
        {
            throw new NotImplementedException();
        }

        public string GetOsSummary()
        {
            throw new NotImplementedException();
        }
        public string GetOsDescription()
        {
            throw new NotImplementedException();
        }
    }
}