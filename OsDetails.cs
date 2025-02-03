using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class OsDetails : IHardwareComponent
    {
        public string Name { get; }
        public string Manufacturer { get; }
        public string Version { get; }
        public string Architecture { get; }

        private OsDetails(OsDetailsBuilder builder)
        {
            Name = builder.Name;
            Manufacturer = builder.Manufacturer;
            Version = builder.Version;
            Architecture = builder.Architecture;
        }

        public class OsDetailsBuilder
        {
            public string Name { get; set; } = "Unknown";
            public string Manufacturer { get; set; } = "Unknown";
            public string Version { get; set; } = "Unknown";
            public string Architecture { get; set; } = "Unknown";

            public OsDetails Build() => new (this);
        }

        public string GetSummary()
        {
            return $"{Name} {Architecture} made by {Manufacturer} ({Version})";
        }

        public string GetDescription()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine("\nInformation about OS(Operating System):");
            stringBuilder.AppendLine("Name: ");
            stringBuilder.Append(Name);
            stringBuilder.AppendLine("Architecture: ");
            stringBuilder.Append(Architecture);
            stringBuilder.AppendLine("Version: ");
            stringBuilder.Append(Version);
            stringBuilder.AppendLine("Manufacturer: ");
            stringBuilder.Append(Manufacturer);

            return stringBuilder.ToString();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}