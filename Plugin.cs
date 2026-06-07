using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace AdjustableTraderRows
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.JustNU.AdjustableTraderRows";
        public const string PluginName = "Adjustable Trader Rows";
        public const string PluginVersion = "1.0.1";

        internal static ConfigEntry<int> TraderColumns { get; private set; }
        internal static ConfigEntry<float> ScanInterval { get; private set; }
        internal static ConfigEntry<bool> DebugLogging { get; private set; }

        private void Awake()
        {
            TraderColumns = Config.Bind(
                "General",
                "Trader Columns",
                8,
                new ConfigDescription(
                    "Number of trader cards to display per row on the trader selection screen.",
                    new AcceptableValueRange<int>(1, 20)));

            ScanInterval = Config.Bind(
                "Advanced",
                "Scan Interval Seconds",
                0.50f,
                new ConfigDescription(
                    "How often the plugin scans the active UI for the trader-card grid.",
                    new AcceptableValueRange<float>(0.10f, 5.00f)));

            DebugLogging = Config.Bind(
                "Advanced",
                "Debug Logging",
                false,
                "Enable verbose logging when the plugin applies trader-grid layout changes.");

            var runner = new GameObject("AdjustableTraderRows.RuntimeScanner");
            DontDestroyOnLoad(runner);
            runner.hideFlags = HideFlags.HideAndDontSave;
            runner.AddComponent<TraderRowsRuntimeScanner>();

            Logger.LogInfo($"Plugin {PluginGuid} is loaded. Trader Columns = {TraderColumns.Value}");
        }
    }
}
