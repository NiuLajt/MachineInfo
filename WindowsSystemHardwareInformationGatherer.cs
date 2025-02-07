using Microsoft.Win32;
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
            HardwareData hardwareData = new()
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
                Name = GetCpuName(),
                Manufacturer = GetCpuManufacturer(),
                CoresCount = GetCpuCores(),
                ThreadsCount = GetCpuThreads(),
                BaseClockFrequency = GetCpuClock(),
                Architecture = GetCpuArchitecture() switch
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
                L1Cache = GetCpuL1CacheCapacity(),
                L2Cache = GetCpuL2CacheCapacity(),
                L3Cache = GetCpuL3CacheCapacity(),
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
                VideoMemoryAmount = GetGpuMemoryCapacity() / (1024 * 1024),
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



        private static string GetCpuName()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj["Name"]?.ToString() ?? "Unknown";
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetCpuName()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }

        private static string GetCpuManufacturer()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Manufacturer FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj["Manufacturer"]?.ToString() ?? "Unknown";
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetCpuManufacturer()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }

        private static string GetCpuArchitecture()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Architecture FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    var architecture = obj["Architecture"];
                    if (architecture != null)
                    {
                        switch ((ushort)architecture)
                        {
                            case 0:
                                return "x86";
                            case 1:
                                return "MIPS";
                            case 2:
                                return "Alpha";
                            case 3:
                                return "PowerPC";
                            case 6:
                                return "IA64";
                            case 9:
                                return "x64";
                            default:
                                return "Unknown";
                        }
                    }
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetCpuArchitecture()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }

        private static int GetCpuCores()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT NumberOfCores FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj["NumberOfCores"] != null ? Convert.ToInt32(obj["NumberOfCores"]) : 0;
                }
            }
            catch (ManagementException exception) 
            {
                Console.WriteLine($"Error WMI(GetCpuCores()): {exception.Message} {exception.Source}");
            }
            
            return 0;
        }

        private static int GetCpuThreads()
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT NumberOfLogicalProcessors FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj["NumberOfLogicalProcessors"] != null ? Convert.ToInt32(obj["NumberOfLogicalProcessors"]) : 0;
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetCpuThreads()): {exception.Message} {exception.Source}");
            }

            return 0;
        }

        private static int GetCpuL1CacheCapacity()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            var L1Cache = key?.GetValue("");
        }


        private static int GetRamPropertyInt(string propertyName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj[propertyName] != null ? Convert.ToInt32(obj[propertyName]) : 0;
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetRamPropertyInt()): {exception.Message} {exception.Source}");
            }

            return 0;
        }

        private static int GetRamCapacity()
        {
            try
            {
                int totalCapacity = 0;
                using var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    // amount of memory is given in bytes.
                    // There are many many bytes in modern machine so use long type for source value and convert it to gigabytes, then return gigabytes count
                    long bytesInGigabyte = 1073741824; 
                    long bytes = obj["Capacity"] != null ? Convert.ToInt64(obj["Capacity"]) : 0;
                    long gigabytes = bytes / bytesInGigabyte;
                    totalCapacity += Convert.ToInt32(gigabytes);
                }
                return totalCapacity;
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetRamCapacity()): {exception.Message} {exception.Source}");
            }

            return 0;
        }

        private static string GetRamType()
        {
            try
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
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetRamType()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }

        private static int GetRamLatency()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            var L1Cache = key?.GetValue("CAS_Latency");
            if (L1Cache != null) return (int)L1Cache;
            else return 0;
        }

        private static string GetGpuProperty(string propertyName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj[propertyName]?.ToString() ?? "Unknown";
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetGpuProperty()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }


        private static int GetGpuMemoryCapacity()
        {
            try
            {
                int totalCapacity = 0;
                using var searcher = new ManagementObjectSearcher("SELECT AdapterRAM FROM Win32_VideoController");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    totalCapacity += Convert.ToInt32(obj["Capacity"]) / (1024 * 1024 * 1024);
                }
                return totalCapacity;
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetGpuMemoryCapacity()): {exception.Message} {exception.Source}");
            }

            return 0;
        }

        public static string GetGpuMemoryType()
        {
            try
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
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetGpuMemoryType()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }


        private static string GetOsProperty(string propertyName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get().Cast<ManagementObject>())
                {
                    return obj[propertyName]?.ToString() ?? "Unknown";
                }
            }
            catch (ManagementException exception)
            {
                Console.WriteLine($"Error WMI(GetOsProperty()): {exception.Message} {exception.Source}");
            }

            return "Unknown";
        }
    }
}