using MachineInfo;
using System.Text;
using System.Text.Json;

internal class CpuDetails : IHardwareComponent
{
    public string Name { get; }
    public string Manufacturer { get; }
    public int CoresCount { get; }
    public int ThreadsCount { get; }
    public float BaseClockFrequency { get; }
    public string Architecture { get; }
    public float L1Cache { get; }
    public float L2Cache { get; }
    public float L3Cache { get; }


    // Constructor uses builder pattern to create objects
    private CpuDetails(CpuDetailsBuilder builder)
    {
        Name = builder.Name;
        Manufacturer = builder.Manufacturer;
        CoresCount = builder.CoresCount;
        ThreadsCount = builder.ThreadsCount;
        BaseClockFrequency = builder.BaseClockFrequency;
        Architecture = builder.Architecture;
        L1Cache = builder.L1Cache;
        L2Cache = builder.L2Cache;
        L3Cache = builder.L3Cache;
    }

    // Builder class to simplify object creation
    public class CpuDetailsBuilder
    {
        public string Name { get; set; } = "Unknown";
        public string Manufacturer { get; set; } = "Unknown";
        public int CoresCount { get; set; } = 0;
        public int ThreadsCount { get; set; } = 0;
        public float BaseClockFrequency { get; set; } = 0;
        public string Architecture { get; set; } = "Unknown";
        public float L1Cache { get; set; } = 0;
        public float L2Cache { get; set; } = 0;
        public float L3Cache { get; set; } = 0;

        public CpuDetails Build() => new(this);
    }


    public string GetSummary()
    {
        return $"({Architecture}){Name} with {CoresCount} cores, {ThreadsCount} threads and {BaseClockFrequency} GHz of clock frequency.";
    }

    public string GetDescription()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine("\nInformation about CPU(Central Processing Unit):");
        stringBuilder.AppendLine("Model: ");
        stringBuilder.Append(Name);
        stringBuilder.AppendLine("Architecture: ");
        stringBuilder.Append(Architecture);
        stringBuilder.AppendLine("Cores(physical processing units): ");
        stringBuilder.Append(CoresCount);
        stringBuilder.AppendLine("Threads(Logical processing units): ");
        stringBuilder.Append(ThreadsCount);
        stringBuilder.AppendLine("Base Clock: ");
        stringBuilder.Append(BaseClockFrequency);
        stringBuilder.Append(" GHz");
        stringBuilder.AppendLine("L1 cache memory: ");
        stringBuilder.Append(L1Cache);
        stringBuilder.AppendLine("L2 cache memory: ");
        stringBuilder.Append(L2Cache);
        stringBuilder.AppendLine("L3 cache memory: ");
        stringBuilder.Append(L3Cache);
        stringBuilder.AppendLine("Made by: ");
        stringBuilder.Append(Manufacturer);

        return stringBuilder.ToString();
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}