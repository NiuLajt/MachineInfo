using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MachineInfo
{
    internal class RamDetails : IHardwareComponent
    {
        public int MemoryAmount { get; }
        public string MemoryType { get; }
        public int MemoryFrequency { get; }
        public int MemoryLatency { get; }

        private RamDetails(RamDetailsBuilder builder)
        {
            MemoryAmount = builder.MemoryAmount;
            MemoryType = builder.MemoryType;
            MemoryFrequency = builder.MemoryFrequency;
            MemoryLatency = builder.MemoryLatency;
        }

        public class RamDetailsBuilder
        {
            public int MemoryAmount { get; set; } = 0;
            public string MemoryType { get; set; } = "Unknown";
            public int MemoryFrequency { get; set; } = 0;
            public int MemoryLatency { get; set; } = 0;

            public RamDetails Build() => new(this);
        }

        public string GetSummary()
        {
            return $"{MemoryAmount} GB of {MemoryType} {MemoryFrequency} Mhz RAM";
        }

        public string GetDescription()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine("\nInformation about RAM(Random Access Memory):");
            stringBuilder.AppendLine("Memory: ");
            stringBuilder.Append(MemoryAmount);
            stringBuilder.Append(" GB");
            stringBuilder.AppendLine("Memory type: ");
            stringBuilder.Append(MemoryType);
            stringBuilder.AppendLine("Memory clock: ");
            stringBuilder.Append(MemoryFrequency);
            stringBuilder.Append(" MHz");
            stringBuilder.AppendLine("Latency: CL");
            stringBuilder.Append(MemoryLatency);

            return stringBuilder.ToString();
        }


        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
