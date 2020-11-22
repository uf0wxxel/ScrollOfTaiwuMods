using System.IO;
using System.Reflection;

namespace UnityModManagerNet
{
    public static class ModLoggerExtensions
    {
        public static void Debug(this UnityModManager.ModEntry.ModLogger logger, string str)
        {
#if DEBUG
            logger.Log(str);
#endif
        }

        public static void DebugFileWriteLine(this UnityModManager.ModEntry.ModLogger logger, string str)
        {
#if DEBUG
            logger.Log(str);
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "debug.log");
            logger.Log(path);
            File.AppendAllLines(path, new[] { str });
#endif
        }
    }
}
