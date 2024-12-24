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
    private const string MULTIPLAYER_TIME_OPTIONS_NAME = "TimeScaleMinSettingButtonOptionPanel";
    private const string BUTTON_OPTIONS_FIELD_NAME = "buttonOptions";

    internal static ManualLogSource sLogger;

    private ButtonOptionSelect mFoundOptions;

    #region Unity Code

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Function")]
    private void Awake()
    {
        var file = new FileInfo(typeof(MoreSpeedsHost).Assembly.Location);
        
        sLogger = base.Logger;
        sLogger.LogInfo($"Loaded {MyPluginInfo.PLUGIN_GUID}.");
        sLogger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} created at {file.CreationTime}.");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Function")]
    private void LateUpdate()
    {
        // In multiplayer, the options are added after the scene is loaded
        // Take the easy way out and just keep looking for it until we find one
        if (mFoundOptions != null)
        {
            return;
        }

        if (this.SearchForTimeOptions())
        {
            sLogger.LogInfo("Setting up additional time options.");
            this.AddCustomSpeedOptions(mFoundOptions);
        }
    }

    #endregion

    #region Plugin Code

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset
        mFoundOptions = null;
    }

    private bool SearchForTimeOptions()
    {
        // Unfortunately, the only way to uniquely identify the multiplayer one is through its name
        foreach (var select in GameObject.FindObjectsOfType<ButtonOptionSelect>())
        {
            if (string.Equals(select.name, MULTIPLAYER_TIME_OPTIONS_NAME, System.StringComparison.OrdinalIgnoreCase))
            {
                mFoundOptions = select;
                return true;
            }
        }

        // Check if we have a settings manager in the scene
        var settingsManager = GameObject.FindObjectsOfType<GameSettingsManager>()
            .OfType<IGameSettingsManager>().FirstOrDefault();

        if (settingsManager != null && settingsManager is GameSettingsManager)
        {
            var settingsPanel = GameObject.FindObjectOfType<GameSettingsPanel>();
            if (settingsPanel != null)
            {
                mFoundOptions = settingsPanel.timeScaleMinSelect;
                return true;
            }
        }

        return false;
    }

    private void AddCustomSpeedOptions(ButtonOptionSelect select)
    {
        // We can't get buttonOptions directly due to some strange issue
        // Reflection time!

        var type = typeof(ButtonOptionSelect);
        var buttonOptions = type.GetField(BUTTON_OPTIONS_FIELD_NAME);
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