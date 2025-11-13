using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublePendulum
{
    internal class SettingsManager
    {
        private string filePath;

        public SettingsManager(string filePath)
        {
            this.filePath = filePath + ".json";
        }

        public void SaveSettings(Settings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public Settings LoadSettings()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Settings>(json);
            }
            else
            {
                return new Settings(50, 50, 0, 0, false, 1, 1, 1, 9.81); // Return default settings if file doesn't exist
            }
        }
    }
}
