using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MachineInfo
{
    internal class HardwareDataStorage : IHardwareDataStorage
    {
        private readonly string _path;
        private readonly HardwareData _data;

        public HardwareDataStorage(string path, HardwareData data)
        {
            _path = path;
            _data = data;
        }

        public void SaveDataToCacheFile()
        {
            var json = JsonSerializer.Serialize<HardwareData>(_data);
            File.WriteAllText(_path, json);
        }
    }
}