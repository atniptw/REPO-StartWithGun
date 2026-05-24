using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartWithGun;

[BepInPlugin("UsagiDev.StartWithGun", "StartWithGun", "1.0.3")]
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
            new ConfigDescription(
                "The item to receive at the start of each run.",
                new AcceptableValueList<string>(
                    // Carts
                    "Item Cart Medium",
                    "Item Cart Small",
                    // Drones
                    "Item Drone Battery",
                    "Item Drone Feather",
                    "Item Drone Indestructible",
                    "Item Drone Torque",
                    "Item Drone Zero Gravity",
                    // Grenades
                    "Item Grenade Duct Taped",
                    "Item Grenade Explosive",
                    "Item Grenade Human",
                    "Item Grenade Shockwave",
                    "Item Grenade Stun",
                    // Guns
                    "Item Gun Handgun",
                    "Item Gun Shotgun",
                    "Item Gun Tranq",
                    // Health
                    "Item Health Pack Large",
                    "Item Health Pack Medium",
                    "Item Health Pack Small",
                    // Melee
                    "Item Melee Baseball Bat",
                    "Item Melee Frying Pan",
                    "Item Melee Inflatable Hammer",
                    "Item Melee Sledge Hammer",
                    "Item Melee Sword",
                    // Mines
                    "Item Mine Explosive",
                    "Item Mine Shockwave",
                    "Item Mine Stun",
                    // Misc
                    "Item Extraction Tracker",
                    "Item Orb Zero Gravity",
                    "Item Power Crystal",
                    "Item Rubber Duck",
                    "Item Valuable Tracker",
                    // Upgrades
                    "Item Upgrade Map Player Count",
                    "Item Upgrade Player Energy",
                    "Item Upgrade Player Extra Jump",
                    "Item Upgrade Player Grab Range",
                    "Item Upgrade Player Grab Strength",
                    "Item Upgrade Player Health",
                    "Item Upgrade Player Sprint Speed",
                    "Item Upgrade Player Tumble Launch"
                )
            )
        );

        Patch();
        SceneManager.sceneLoaded += OnSceneLoaded;

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
