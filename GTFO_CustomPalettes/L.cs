using BepInEx.Logging;
using System;

namespace CustomPalettes
{
    internal static class L
    {
        internal static ManualLogSource Logger { private get; set; }

        internal static void Info(string msg)
        {
            Logger.LogInfo(msg);
        }

        internal static void Msg(string msg)
        {
            Logger.LogMessage(msg);
        }

        internal static void Debug(string msg)
        {
            Logger.LogDebug(msg);
        }

        internal static void Warning(string msg)
        {
            Logger.LogWarning(msg);
        }

        internal static void Error(string msg)
        {
            Logger.LogError(msg);
        }

        internal static void Exception(Exception ex)
        {
            Logger.LogError(ex.Message);
            Logger.LogWarning("StackTrace:\n" + ex.StackTrace);
        }
    }
}
