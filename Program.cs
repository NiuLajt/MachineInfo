using Hardware.Info;
using MachineInfo;
using System.Management;
using System.Runtime.Versioning;


const string path = "machineinfo_cache.json";
FileHardwareInformationGatherer fileGatherer = new(path);
HardwareInformationManager manager = new(fileGatherer);
HardwareData data;
if (manager.GetHardwareData() == null) // when anything goes wrong (empty file or something) this method returns null value.
{
    Console.WriteLine("magic happens");
    manager._gatherer = new WindowsSystemHardwareInformationGatherer(); // then thats needed to assign another gatherer that gets data from system API, not from file
    data = manager.GetHardwareData();
    HardwareDataStorage dataStorage = new(path, data);
    dataStorage.SaveDataToCacheFile(); // gathered data should be save to file for future
}
else
{
    Console.WriteLine("file logic happens");
    data = manager.GetHardwareData(); // if everything work as expected (file is ok) we use file to gather hardware data because thats faster
}


// Command line arguments handling (arguments are optional, in there are no arguments then display general info)
if (args.Length == 0)
{
    Console.WriteLine("MACHINE SPECS:");
    Console.WriteLine("CPU: " + manager.GetCpuSummary());
    Console.WriteLine("RAM: " + manager.GetRamSummary());
    Console.WriteLine("GPU: " + manager.GetGpuSummary());
    Console.WriteLine("OS: " + manager.GetOsSummary());
}
else if (args.Length == 1)
{
    switch (args[0])
    {
        case "-c":
            Console.WriteLine(manager.GetCpuDescription());
            break;

        case "-m":
            Console.WriteLine(manager.GetGpuDescription());
            break;

        case "-g":
            Console.WriteLine(manager.GetRamDescription());
            break;

        case "-s":
            Console.WriteLine(manager.GetOsDescription());
            break;


        default:
            Console.WriteLine("DUPA");
            break;
    }
}
else
{
    Console.WriteLine("DUPA DUPA DUPA");
}