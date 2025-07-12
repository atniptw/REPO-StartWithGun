using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartWithGun;

[BepInPlugin("UsagiDev.StartWithGun", "StartWithGun", "1.0.0")]
public class StartWithGun : BaseUnityPlugin
{
    internal static StartWithGun Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    static ConfigEntry<string> gun;

    private void Awake()
    {
        Instance = this;

        // Prevent the plugin from being deleted
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        gun = this.Config.Bind("Default Item Asset Name", "Gun", "Item Gun Handgun");
        Patch();
        SceneManager.sceneLoaded += OnSceneLoaded;

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SemiFunc.RunIsLevel() && SemiFunc.IsMasterClientOrSingleplayer())
        {
            var purchased = StatsManager.instance.itemsPurchased;
            var itemAssetName = gun.Value;
            if (purchased.TryGetValue(itemAssetName, out var count) && count == 0)
            {
                purchased[itemAssetName]++;
                StatsManager.instance.itemsPurchasedTotal[itemAssetName]++;
                Logger.LogInfo($"Added {itemAssetName}");
            }
        }
    }

    internal void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
    }

    internal void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }
}