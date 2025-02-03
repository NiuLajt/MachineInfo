using Hardware.Info;
using System.Management;

var hardwareInfo = new HardwareInfo();
hardwareInfo.RefreshAll();

Console.WriteLine("\nHARDWARE OF THIS MACHINE:");

string cpuInformationString = "CPU info: ";
foreach (var cpu in hardwareInfo.CpuList)
{
    cpuInformationString += cpu.Name;
    cpuInformationString += " with ";
    cpuInformationString += cpu.NumberOfCores;
    cpuInformationString += " cores ";
    cpuInformationString += cpu.NumberOfLogicalProcessors;
    cpuInformationString += " threads ";
}

int ramMemoryAmount = 0;
string memoryInformationString = "RAM info: ";
foreach (var memory in hardwareInfo.MemoryList)
{
    ramMemoryAmount += (int)Math.Round(memory.Capacity / 1024.0 / 1024 / 1024, 2);
}
memoryInformationString += ramMemoryAmount;
memoryInformationString += " GB DDR4";

string gpuInformationString = "GPU info: ";
foreach (var gpu in hardwareInfo.VideoControllerList)
{
    gpuInformationString += gpu.Name;
    gpuInformationString += " with ";
    gpuInformationString += Math.Round(gpu.AdapterRAM / 1024.0 / 1024 / 1024, 2);
    gpuInformationString += " GB of memory";
}


//string driveInformationString = "Drive info: ";
//bool firstDrive = true;
//List<string> drives = new List<string>();
//using (var searcher = new ManagementObjectSearcher("SELECT Model, Size, MediaType FROM Win32_DiskDrive"))
//{
//    foreach (ManagementObject drive in searcher.Get())
//    {
//        string model = drive["Model"]?.ToString() ?? "Unknown";
//        string mediaType = drive["MediaType"]?.ToString() ?? "Unknown";
//        long sizeInBytes = drive["Size"] != null ? Convert.ToInt64(drive["Size"]) : 0;
//        double sizeInGigabytes = Math.Round(sizeInBytes / 1024.0 / 1024 / 1024, 0);
//        string interfaceType = drive["InterfaceType"]?.ToString() ?? "Unknown";
//        string type;
//        if (mediaType.Contains("SSD") || interfaceType == "NVMe") type = "SSD";
//        else type = "HDD";

//        if (!firstDrive) driveInformationString += ", ";
//        firstDrive = false;

//        driveInformationString += $"{sizeInGigabytes} GB {type}";
//    }
//}

Console.WriteLine(cpuInformationString);
Console.WriteLine(memoryInformationString);
Console.WriteLine(gpuInformationString);
// Console.WriteLine(driveInformationString);
Console.WriteLine("OS: Windows 11 Pro (64-bit)");