namespace UnityModManagerNet
{
    internal static class DateFileExtensions
    {
        public static string SetColor(this DateFile dateFile, TaiwuTextColor color, string text, bool noChange = false)
        {
            return dateFile.SetColoer((int)color, text, noChange);
        }

        public static string SetTaiwuColor(this string text, TaiwuTextColor color)
        {
            return DateFile.instance.SetColoer((int)color, text);
        }
    }

    /// <summary>
    /// Colors are extracted from DateFile.instance.massageDate
    /// </summary>
    internal enum TaiwuTextColor
    {
        Black = 10000,
        DeepGray = 10001,
        DeepBrown = 10002,
        LightBrown = 10003,
        DarkRed = 10004,
        LightYellow = 10005,
        Pink = 10006,
        Wheat = 20001,
        LightGrey = 20002,
        White = 20003,
        LightGreen = 20004,
        LightBlue = 20005,
        Cyan = 20006,
        DarkPurple = 20007,
        Yellow = 20008,
        Orange = 20009,
        Red = 20010,
        Golden = 20011,
    }
}
