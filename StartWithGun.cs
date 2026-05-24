using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartWithGun;

[BepInPlugin("UsagiDev.StartWithGun", "StartWithGun", "1.0.2")]
[BepInDependency("nickklmao.menulib")]
public class StartWithGun : BaseUnityPlugin
{
    internal static StartWithGun Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    internal static ConfigEntry<string> gun = null!;

    private void Awake()
    {
        Instance = this;

        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        gun = this.Config.Bind(
            "Default Item Asset Name",
            "Gun",
            "Item Gun Handgun",
            new ConfigDescription("The item to receive at the start of each run.", null, "HideFromREPOConfig")
        );

        Patch();
        SceneManager.sceneLoaded += OnSceneLoaded;
        ItemSelectionMenu.Register();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SemiFunc.RunIsLevel() && SemiFunc.IsMasterClientOrSingleplayer())
        {
            var itemAssetName = gun.Value;
            if (ItemGranter.TryGrantItem(
                StatsManager.instance.itemsPurchased,
                StatsManager.instance.itemsPurchasedTotal,
                itemAssetName))
            {
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
