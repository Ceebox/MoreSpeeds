using BepInEx;
using BepInEx.Logging;
using System.IO;
using UnityEngine;

namespace MoreSpeeds;

/// <summary>
/// Basic plugin host for the MoreSpeeds plugin.
/// </summary>
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MoreSpeedsHost : BaseUnityPlugin
{
    internal static ManualLogSource sLogger;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Function")]
    private void Awake()
    {
        var file = new FileInfo(typeof(MoreSpeedsHost).Assembly.Location);
        
        sLogger = base.Logger;
        sLogger.LogInfo($"Loaded {MyPluginInfo.PLUGIN_GUID}::{file.CreationTime}.");
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Function")]
    private void Update()
    {
        Debug.Log("Hello");
        sLogger.LogInfo($"Updating {MyPluginInfo.PLUGIN_GUID}");
    }
}
