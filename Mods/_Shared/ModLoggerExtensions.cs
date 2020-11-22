using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace UnityModManagerNet
{
    internal static class ModLoggerExtensions
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
        };

        public static void Debug(this UnityModManager.ModEntry.ModLogger logger, string str)
        {
#if DEBUG
            logger.Log(str);
#endif
        }

        public static void DebugFileWriteLine(this UnityModManager.ModEntry.ModLogger logger, string str)
        {
#if DEBUG
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "debug.log");
            File.AppendAllLines(path, new[] { str });
#endif
        }

        public static void DebugFileWriteJson<T>(this UnityModManager.ModEntry.ModLogger logger, string fileNameWithoutExtension, T obj)
        {
#if DEBUG
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{fileNameWithoutExtension}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(obj, _jsonSerializerSettings));
#endif
        }
    }
}
