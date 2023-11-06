using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ocelot.Configuration.File;

namespace ApiGateway.Extensions
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddOcelotConfigFiles(this IConfigurationBuilder builder, string folder, string[] appNames, IWebHostEnvironment env)
        {
            const string primaryConfigFile = "ocelot.json";
            string globalConfigFile = "common.ocelot." + env.EnvironmentName.ToLower() + ".json";

            var files = new DirectoryInfo(folder)
                            .EnumerateFiles()
                            .Where(f => f.Name.ToLower().Contains($"ocelot.{env.EnvironmentName.ToLower()}.json") && appNames.Any(e => f.Name.Contains(e.ToLower())))
                            .ToList();

            var fileConfiguration = new FileConfiguration();
            foreach (var f in files)
            {

                if (files.Count() > 1 && f.Name.Equals(primaryConfigFile, StringComparison.OrdinalIgnoreCase))
                    continue;

                var lines = File.ReadAllText(f.FullName);
                var config = JsonConvert.DeserializeObject<FileConfiguration>(lines);
                if (f.Name.Equals(globalConfigFile, StringComparison.OrdinalIgnoreCase))
                    fileConfiguration.GlobalConfiguration = config.GlobalConfiguration;

                fileConfiguration.Aggregates.AddRange(config.Aggregates);
                fileConfiguration.Routes.AddRange(config.Routes);
            }

            var json = JsonConvert.SerializeObject(fileConfiguration);
            File.WriteAllText(primaryConfigFile, json);
            builder.AddJsonFile(primaryConfigFile, false, false);
            return builder;
        }
    }
}
