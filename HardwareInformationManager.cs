using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class HardwareInformationManager
    {
        public IHardwareInformationsGatherer _gatherer;

        public HardwareInformationManager(IHardwareInformationsGatherer gatherer)
        {
            _gatherer = gatherer;
        }


        public HardwareData GetHardwareData() => _gatherer.GetHardwareData();
        public CpuDetails GetCpuDetails() => _gatherer.GetCpuDetails();
        public RamDetails GetRamDetails() => _gatherer.GetRamDetails();
        public GpuDetails GetGpuDetails() => _gatherer.GetGpuDetails();
        public OsDetails GetOsDetails() => _gatherer.GetOsDetails();


        public string GetCpuSummary()
        {
            var cpuDetails = _gatherer.GetCpuDetails();
            return cpuDetails.GetSummary();
        }

        public string GetCpuDescription()
        {
            var cpuDetails = _gatherer.GetCpuDetails();
            return cpuDetails.GetDescription();
        }

        public string GetRamSummary()
        {
            var ramDetails = _gatherer.GetRamDetails();
            return ramDetails.GetSummary();
        }
        public string GetRamDescription()
        {
            var ramDetails = _gatherer.GetRamDetails();
            return ramDetails.GetDescription();
        }

        public string GetGpuSummary()
        {
            var gpuDetails = _gatherer.GetGpuDetails();
            return gpuDetails.GetSummary();
        }

        public string GetGpuDescription()
        {
            var gpuDetails = _gatherer.GetGpuDetails();
            return gpuDetails.GetDescription();
        }

        public string GetOsSummary()
        {
            var osDetails = _gatherer.GetOsDetails();
            return osDetails.GetSummary();
        }

        public string GetOsDescription()
        {
            var osDetails = _gatherer.GetOsDetails();
            return osDetails.GetDescription();
        }
    }
}