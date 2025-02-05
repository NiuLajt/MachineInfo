using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class FileHardwareInformationGatherer : IHardwareInformationsGatherer
    {
        private readonly string _path;

        public FileHardwareInformationGatherer(string _path)
        {
            this._path = _path;
        }

        public CpuDetails GetCpuDetails()
        {
            var hardwareData = GetHardwareDataFromJsonFile();
            if (hardwareData == null)
            {
                var cpuDetails = new CpuDetails.CpuDetailsBuilder().Build();
                return cpuDetails;
            }
            else
            {
                return hardwareData.Cpu;
            }
        }

        public RamDetails GetRamDetails()
        {
            var hardwareData = GetHardwareDataFromJsonFile();
            if (hardwareData == null)
            {
                var ramDetails = new RamDetails.RamDetailsBuilder().Build();
                return ramDetails;
            }
            else
            {
                return hardwareData.Ram;
            }
        }

        public GpuDetails GetGpuDetails()
        {
            var hardwareData = GetHardwareDataFromJsonFile();
            if (hardwareData == null)
            {
                var gpuDetails = new GpuDetails.GpuDetailsBuilder().Build();
                return gpuDetails;
            }
            else
            {
                return hardwareData.Gpu;
            }
        }

        public OsDetails GetOsDetails()
        {
            var hardwareData = GetHardwareDataFromJsonFile();
            if (hardwareData == null)
            {
                var osDetails = new OsDetails.OsDetailsBuilder().Build();
                return osDetails;
            }
            else
            {
                return hardwareData.Os;
            }
        }

        public HardwareData? GetHardwareDataFromJsonFile()
        {
            if (!File.Exists(_path)) throw new FileNotFoundException();
            else
            {
                try
                {
                    string json = File.ReadAllText(_path);
                    return JsonSerializer.Deserialize<HardwareData>(json);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}