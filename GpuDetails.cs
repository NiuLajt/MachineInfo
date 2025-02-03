using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class GpuDetails : IHardwareComponent
    {
        public string Name { get; }
        public string Manufacturer { get; }
        public int VideoMemoryAmount { get; }
        public string VideoMemoryType { get; }

        // Constructor uses builder pattern to create objects
        private GpuDetails(GpuDetailsBuilder builder)
        {
            Name = builder.Name;
            Manufacturer = builder.Manufacturer;
            VideoMemoryAmount = builder.VideoMemoryAmount;
            VideoMemoryType = builder.VideoMemoryType;
        }

        // Builder class to simplify object creation
        public class GpuDetailsBuilder
        {
            public string Name { get; set; } = "Unknown";
            public string Manufacturer { get; set; } = "Unknown";
            public int VideoMemoryAmount { get; set; } = 0;
            public string VideoMemoryType { get; set;} = "Unknown";

            public GpuDetails Build() => new(this);
        }

        public string GetSummary()
        {
            return $"{Manufacturer} {Name} with {VideoMemoryAmount} GB of {VideoMemoryType} VRAM";
        }

        public string GetDescription()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine("\nInformation about GPU(Graphics Processing Unit):");
            stringBuilder.AppendLine("Model: ");
            stringBuilder.Append(Name);
            stringBuilder.AppendLine("Video memory: ");
            stringBuilder.Append(VideoMemoryAmount);
            stringBuilder.Append(" GHz");
            stringBuilder.AppendLine("Video memory type: ");
            stringBuilder.Append(VideoMemoryType);
            stringBuilder.AppendLine("Made by: ");
            stringBuilder.Append(Manufacturer);

            return stringBuilder.ToString();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}