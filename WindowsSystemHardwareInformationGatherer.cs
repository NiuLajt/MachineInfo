using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    [SupportedOSPlatform("windows")]
    internal class WindowsSystemHardwareInformationGatherer : IHardwareInformationsGatherer
    {
        public HardwareData GetHardwareData()
        {
            HardwareData hardwareData = new HardwareData
            {
                Cpu = GetCpuDetails(),
                Gpu = GetGpuDetails(),
                Ram = GetRamDetails(),
                Os = GetOsDetails()
            };
            return hardwareData;
        }

        public CpuDetails GetCpuDetails()
        {
            CpuDetails.CpuDetailsBuilder builder = new()
            {
                Name = GetCpuProperty("Name"),
                Manufacturer = GetCpuProperty("Manufacturer"),
                CoresCount = GetCpuPropertyInt("NumberOfCores"),
                ThreadsCount = GetCpuPropertyInt("ThreadCount"),
                BaseClockFrequency = GetCpuPropertyInt("MaxClockSpeed") / 1000,
                Architecture = GetCpuProperty("Architecture") switch
                {
                    "0" => "x86",
                    "1" => "MIPS",
                    "2" => "Alpha",
                    "3" => "PowerPC",
                    "5" => "ARM",
                    "6" => "Itanium-based",
                    "9" => "x64",
                    _ => "Unknown"
                },
                L1Cache = GetCpuPropertyInt("L1CacheSize"),
                L2Cache = GetCpuPropertyInt("L2CacheSize"),
                L3Cache = GetCpuPropertyInt("L3CacheSize"),
            };
            return builder.Build();
        }

        public RamDetails GetRamDetails()
        {
            RamDetails.RamDetailsBuilder builder = new()
            {
                MemoryAmount = GetRamCapacity(),
                MemoryType = GetRamType(),
                MemoryFrequency = GetRamPropertyInt("Speed"),
                MemoryLatency = GetRamLatency()
            };
            return builder.Build();
        }

        public GpuDetails GetGpuDetails()
        {
            GpuDetails.GpuDetailsBuilder builder = new()
            {
                Name = GetGpuProperty("Name"),
                Manufacturer = GetGpuProperty("AdapterCompatibility"),
                VideoMemoryAmount = GetGpuPropertyInt("AdapterRAM") / (1024 * 1024),
                VideoMemoryType = GetGpuMemoryType(),
            };
            return builder.Build();
        }

        public OsDetails GetOsDetails()
        {
            OsDetails.OsDetailsBuilder builder = new()
            {
                Name = GetOsProperty("Caption"),
                Manufacturer = GetOsProperty("Manufacturer"),
                Version = GetOsProperty("Version"),
                Architecture = GetOsProperty("OSArchitecture")
            };
            return builder.Build();
        }


        private static string GetCpuProperty(string propertyName)
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                return obj[propertyName]?.ToString() ?? "Unknown";
            }
            return "Unknown";
        }

        private static int GetCpuPropertyInt(string propertyName)
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                return obj[propertyName] != null ? Convert.ToInt32(obj[propertyName]) : 0;
            }
            return 0;
        }

        private static int GetRamPropertyInt(string propertyName)
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                return obj[propertyName] != null ? Convert.ToInt32(obj[propertyName]) : 0;
            }
            return 0;
        }

        private static int GetRamCapacity()
        {
            int totalCapacity = 0;
            using var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                totalCapacity += Convert.ToInt32(obj["Capacity"]) / (1024 * 1024 * 1024);
            }
            return totalCapacity;
        }

        private static string GetRamType()
        {
            using var searcher = new ManagementObjectSearcher("SELECT MemoryType FROM Win32_PhysicalMemory");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                if (obj["MemoryType"] != null)
                {
                    int type = Convert.ToInt32(obj["MemoryType"]);
                    return type switch
                    {
                        20 => "DDR",
                        21 => "DDR2",
                        22 => "DDR2 FB-DIMM",
                        24 => "DDR3",
                        26 => "DDR4",
                        34 => "DDR5",
                        _ => "Unknown"
                    };
                }
            }
            return "Unknown";
        }

        private static int GetRamLatency()
        {
            using var searcher = new ManagementObjectSearcher("SELECT ConfiguredClockSpeed, ConfiguredCASLatency FROM Win32_PhysicalMemory");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                if (obj["ConfiguredCASLatency"] != null)
                {
                    return (int)obj["ConfiguredCASLatency"];
                }
            }
            return 0;
        }

        private static string GetGpuProperty(string propertyName)
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                return obj[propertyName]?.ToString() ?? "Unknown";
            }
            return "Unknown";
        }

        private static int GetGpuPropertyInt(string propertyName)
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                return obj[propertyName] != null ? Convert.ToInt32(obj[propertyName]) : 0;
            }
            return 0;
        }

        public static string GetGpuMemoryType()
        {
            using var searcher = new ManagementObjectSearcher("SELECT VideoMemoryType FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                if (obj["VideoMemoryType"] != null)
                {
                    int type = Convert.ToInt32(obj["VideoMemoryType"]);
                    return type switch
                    {
                        3 => "DRAM",
                        4 => "VRAM",
                        5 => "SRAM",
                        6 => "WRAM",
                        7 => "EDO RAM",
                        8 => "Burst Synchronous DRAM",
                        9 => "Pipelined Burst SRAM",
                        10 => "CIM Memory",
                        11 => "SGRAM",
                        12 => "GDDR",
                        13 => "GDDR2",
                        14 => "GDDR3",
                        15 => "GDDR4",
                        16 => "GDDR5",
                        17 => "GDDR6",
                        18 => "GDDR6X",
                        _ => "Unknown"
                    };
                }
            }
            return "Unknown";
        }

        private static string GetOsProperty(string propertyName)
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
            {
                return obj[propertyName]?.ToString() ?? "Unknown";
            }
            return "Unknown";
        }
    }
}