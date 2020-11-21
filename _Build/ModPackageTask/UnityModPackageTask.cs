using System;
using System.IO;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace ModPackageTask
{
    public class UnityModPackageTask : Microsoft.Build.Utilities.Task
    {
        private const string RepositoryJsonFileName = "repository.json";
        private const string HostSiteDomain = "taiwumods.vercel.app";

        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        [Required]
        public string OutputPath { get; set; }

        [Required]
        public string PublishPath { get; set; }

        public override bool Execute()
        {
            if (!Directory.Exists(OutputPath))
            {
                Log.LogError($"OutputPath not found: {OutputPath}");
                return false;
            }

            var modInfoPath = Path.Combine(OutputPath, "info.json");
            if (!File.Exists(modInfoPath))
            {
                // Unix FS is case sensitive
                modInfoPath = Path.Combine(OutputPath, "Info.json");
                if (!File.Exists(modInfoPath))
                {
                    Log.LogError($"Mod Info.json not found: {modInfoPath}");
                    return false;
                }
            }

            ModInfo modInfo = null;
            try
            {
                modInfo = JsonConvert.DeserializeObject<ModInfo>(File.ReadAllText(modInfoPath));
                modInfo.Validate();
            }
            catch
            {
                Log.LogError($"Error parsing {modInfoPath}");
                return false;
            }

            var modSettingsPath = Path.Combine(OutputPath, "settings.xml");
            if (File.Exists(modSettingsPath))
            {
                XDocument modSettings = null;
                try
                {
                    modSettings = XDocument.Load(modSettingsPath);
                }
                catch
                {
                    Log.LogError($"Error parsing {modSettingsPath}");
                    return false;
                }
            }

            var modDir = Path.Combine(PublishPath, modInfo.DisplayName);
            var zipFilePath = Path.Combine(modDir, $"{modInfo.Id}.zip");
            EnsureParentDirExists(zipFilePath);

            // Patch info.json
            modInfo.Repository = $"https://{HostSiteDomain}/{Uri.EscapeDataString(modInfo.DisplayName)}/{RepositoryJsonFileName}";
            if (string.IsNullOrEmpty(modInfo.HomePage))
            {
                modInfo.HomePage = $"https://{HostSiteDomain}/{Uri.EscapeDataString(modInfo.DisplayName)}/";
            }

            // Copy readme files
            foreach (var readmeFilePath in Directory.EnumerateFiles(OutputPath, "*.md", SearchOption.TopDirectoryOnly))
            {
                var publishPath = Path.Combine(modDir, Path.GetFileName(readmeFilePath));
                File.Copy(readmeFilePath, publishPath, overwrite: true);
            }

            File.WriteAllText(modInfoPath, JsonConvert.SerializeObject(modInfo, jsonSerializerSettings));

            {
                using var zipFileStream = File.Create(zipFilePath);
                using var outStream = new ZipOutputStream(zipFileStream);

                foreach (var filePath in Directory.EnumerateFiles(OutputPath))
                {
                    outStream.PutNextEntry(new ZipEntry($"{modInfo.Id}/{Path.GetFileName(filePath)}"));
                    using var entryFs = File.OpenRead(filePath);
                    entryFs.CopyTo(outStream);
                }
            }

            var repositoryJsonPath = Path.Combine(modDir, RepositoryJsonFileName);
            EnsureParentDirExists(repositoryJsonPath);
            var repo = new Repository
            {
                Releases = new Release[]
                {
                    new Release
                    {
                        Id = modInfo.Id,
                        Version = modInfo.Version,
                        DownloadUrl = $"https://{HostSiteDomain}/{Uri.EscapeDataString(modInfo.DisplayName)}/{Uri.EscapeDataString(modInfo.Id)}.zip",
                    }
                },
            };

            File.WriteAllText(repositoryJsonPath, JsonConvert.SerializeObject(repo, jsonSerializerSettings));

            return true;
        }

        private static void EnsureParentDirExists(string filePath)
        {
            var parentDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(parentDir))
            {
                Directory.CreateDirectory(parentDir);
            }
        }
    }
}
