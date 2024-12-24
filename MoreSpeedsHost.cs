using BepInEx;
using BepInEx.Logging;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreSpeeds;

/// <summary>
/// Basic plugin host for the MoreSpeeds plugin.
/// </summary>
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MoreSpeedsHost : BaseUnityPlugin
{
    internal static ManualLogSource sLogger;

#region Unity Code

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Function")]
    private void Awake()
    {
        var file = new FileInfo(typeof(MoreSpeedsHost).Assembly.Location);
        
        sLogger = base.Logger;
        sLogger.LogInfo($"Loaded {MyPluginInfo.PLUGIN_GUID}.");
        sLogger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} created at {file.CreationTime}.");
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Function")]
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #endregion

    #region Plugin Code

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we have a settings manager in the scene
        var settingsManager = GameObject.FindObjectsOfType<MonoBehaviour>()
            .OfType<IGameSettingsManager>().FirstOrDefault();

        // TODO: Properly figure out if we're multiplayer (LobbyMultiplayer)
        // Then find our UI buttons and properly trigger our speed

        if (settingsManager is GameSettingsManagerMultiplayer)
        {
            this.SetUpMultiplayerTimeOptions();
        }
        else if (settingsManager is GameSettingsManager)
        {
            this.SetUpLocalTimeOptions();
        }
    }

    private void SetUpLocalTimeOptions()
    {
        sLogger.LogInfo("Setting up local time options.");

        var settingsPanel = GameObject.FindObjectOfType<GameSettingsPanel>();
        if (settingsPanel != null)
        { 
            this.AddCustomSpeedOptions(settingsPanel.timeScaleMinSelect);
        }
    }

    private void SetUpMultiplayerTimeOptions()
    {
        sLogger.LogInfo("Setting up multiplayer time options.");

        var obk = GameObject.FindObjectOfType<ButtonOptionSelect>();
        if (obk != null)
        {
            print(obk.name);
        }
    }

    private void AddCustomSpeedOptions(ButtonOptionSelect select)
    {
        // We can't get buttonOptions directly due to some strange issue
        // Reflection time!

        var type = typeof(ButtonOptionSelect);
        var buttonOptions = type.GetField("buttonOptions");
        List<ButtonOption> options = (List<ButtonOption>) buttonOptions.GetValue(select);

        foreach (var speed in CustomSpeeds.Speeds)
        {
            options.Add(new ButtonOption()
            {
                optionText = speed.Key,
                optionFloatValue = speed.Value,
            });
        }
    }

    #endregion
}