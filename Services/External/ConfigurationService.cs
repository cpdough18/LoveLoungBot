#region

using Newtonsoft.Json;
using Radon.Core;
using System.IO;
using System.Text;

#endregion

namespace Radon.Services
{
    public class ConfigurationService
    {
        private readonly Configuration _configuration;

        public ConfigurationService(Configuration configuration)
        {
            _configuration = configuration;
        }
        public static Configuration LoadNewConfig()
        {
            const string fileName = "config.json";
            if (!File.Exists(fileName))
            {
                File.CreateText(fileName).Close();
                File.WriteAllText(fileName, JsonConvert.SerializeObject(new Configuration()));
            }

            var json = File.ReadAllText(fileName, Encoding.UTF8);
            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}