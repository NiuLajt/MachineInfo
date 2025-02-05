using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class SystemHardwareInformationGatherer : IHardwareInformationsGatherer
    {
        public CpuDetails GetCpuDetails()
        {
            using var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                var builder = new CpuDetails.CpuDetailsBuilder();
                builder.Name = obj["Name"]?.ToString() ?? "Unknown";
                builder.Manufacturer = 

                return new CpuDetails
                {
                    Name = obj["Name"]?.ToString() ?? "Unknown",
                    CoresCount = Convert.ToInt32(obj["NumberOfCores"] ?? 0),
                    ThreadsCount = Convert.ToInt32(obj["ThreadCount"] ?? 0)
                };
            }
            return new CpuDetails();
        }

        public GpuDetails GetGpuDetails()
        {
            throw new NotImplementedException();
        }

        public OsDetails GetOsDetails()
        {
            throw new NotImplementedException();
        }

        public RamDetails GetRamDetails()
        {
            throw new NotImplementedException();
        }
    }
}