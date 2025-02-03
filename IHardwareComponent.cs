using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal interface IHardwareComponent
    {
        public string GetSummary();
        public string GetDescription();
        public string ToJson();
    }
}
