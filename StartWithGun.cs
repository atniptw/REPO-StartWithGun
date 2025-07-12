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

[BepInPlugin("StartWithGun.StartWithGun", "StartWithGun", "1.0")]
public class StartWithGun : BaseUnityPlugin
{
    internal static StartWithGun Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    public static ConfigEntry<bool> isEnabled;

    //public static bool shouldEquip = true;
    private void Awake()
    {
        Instance = this;

        // Prevent the plugin from being deleted
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        isEnabled = this.Config.Bind("StartWithGun", "Enabled", true);
        Patch();
        SceneManager.sceneLoaded += OnSceneLoaded;

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Logger.LogError($"onsceneloaded> scene: {scene.name} mode:{mode} levelcurrent={RunManager.instance.levelCurrent} gameover={RunManager.instance.gameOver} RunisLevel: {SemiFunc.RunIsLevel()}");
        //Logger.LogError($"purchasedgun:{StatsManager.instance.itemsPurchased["Item Gun Handgun"]}");
        if (isEnabled.Value && SemiFunc.RunIsLevel() && SemiFunc.IsMasterClientOrSingleplayer())
        {
            var purchased = StatsManager.instance.itemsPurchased;
            const string itemAssetName = "Item Gun Handgun";
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
    
    private void Update()
    {
        // Code that runs every frame goes here
    }

    /*
    [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.Start))]
    class PlayerAvatar_Start
    {
        [HarmonyPostfix]
        static void Postfix(PlayerAvatar __instance)
        {
            SemiFunc
        }
    }
    */

    /*[HarmonyPatch(typeof(InventorySpot), "Update")]
    class InventorySpot_Patch
    {
        [HarmonyPostfix]
        static void Postfix(InventorySpot __instance)
        {
            if (shouldEquip && SemiFunc.RunIsLevel() && RunManager.instance.loadLevel == 0)
            {
                shouldEquip = false;
                /*
                ItemEquippable[] items = Resources.FindObjectsOfTypeAll<ItemEquippable>();
                foreach (ItemEquippable item in items)
                {
                    if (item.name == "Item Gun Handgun")
                    {
                        Logger.LogError($"Item found {item.name}, trying to equip");
                        ItemEquippable gun = Object.Instantiate<ItemEquippable>(item);
                        gun.name += "_Clone";
                        Logger.LogError($"Spawned {gun.name}");
                        gun.RequestEquip(__instance.inventorySpotIndex);
                        break;
                        //__instance.EquipItem(item);
                    }
                }
                #1#
                var items = Resources.FindObjectsOfTypeAll<Item>();
                foreach (var item in items)
                {
                    if (item.name == "Item Gun Handgun")
                    {
                        var player = Object.FindObjectOfType<PlayerController>();
                        if (player != null)
                        {
                            // var gun = Object.Instantiate<Item>(item, player.transform.position, Quaternion.identity);
                            //gun.RequestEquip(__instance.inventorySpotIndex);
                            // ItemManager.instance.purchasedItems.Add(gun);
                            StatsManager.instance.itemsPurchased[item.itemAssetName]++;
                            StatsManager.instance.itemsPurchasedTotal[item.itemAssetName]++;
                            /*
                            foreach (var p in ItemManager.instance.purchasedItems)
                            {
                                if (p.name == item.name)
                                {
                                    p.
                                }
                            }
                            #1#
                            //Inventory.instance.inventorySpots.ElementAt(0).CurrentItem = gun;
                            //__instance.CurrentItem = gun;
                            // Logger.LogError($"inventory: {Inventory.instance.inventorySpots.First()}");
                            //gun.RequestEquip(__instance.inventorySpotIndex);
                            Logger.LogError($"Found Player {player.name}, spawning {item.itemAssetName}");
                            break;
                        }
                    }
                }
                    
            }
        }
    }*/

    /*
    [HarmonyPatch(typeof(PlayerController), "Awake")]
    class PlayerController_Awake_Patch
    {
        [HarmonyPostfix]
        static void Postfix(PlayerController player)
        {
            GameObject[] items = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject item in items)
            {
                if (item.name == "Item Gun Handgun")
                {
                    GameObject gun = Object.Instantiate<GameObject>(item,
                        player.transform.position + player.transform.forward * 2f, Quaternion.identity);
                    gun.name += "_Clone";
                    Logger.LogInfo("Gun spawned OK");
                }
            }

        }
    }
    [HarmonyPatch(typeof(RunManager))]
    public static class RunManagerPatch
    {
        [HarmonyPatch("ChangeLevel")]
        [HarmonyPrefix]
        private static void ChangeLevelPatch(ref RunManager.ChangeLevelType _changeLevelType)
        {
            //Logger.LogError($"loadlevel={RunManager.instance.loadLevel}  changeLevelType={_changeLevelType} RunisLevel: {SemiFunc.RunIsLevel()}");
            if (isEnabled.Value && RunManager.instance.loadLevel == 0 &&
                SemiFunc.IsMasterClientOrSingleplayer() && _changeLevelType == RunManager.ChangeLevelType.RunLevel)
            {
                StatsManager.instance.itemsPurchased["Item Gun Handgun"]++;
                StatsManager.instance.itemsPurchasedTotal["Item Gun Handgun"]++;
                Logger.LogError("Added Itemgun");
            }
        }
    }
    */
    /*
    [HarmonyPatch(typeof(RunManager))]
    public static class RunManagerPatch
    {
        [HarmonyPatch("ChangeLevel")]
        [HarmonyPrefix]
        private static void ChangeLevelPatch(ref RunManager.ChangeLevelType _changeLevelType)
        {
            if (isEnabled.Value && RunManager.instance.loadLevel == 0 &&
                SemiFunc.IsMasterClientOrSingleplayer() && _changeLevelType == RunManager.ChangeLevelType.RunLevel)
            {
                SemiFunc.StatSetRunCurrency(50);
                _changeLevelType = RunManager.ChangeLevelType.Shop;
                //ItemEquippable gun = new ItemEquippable().
            }
            //Logger.LogInfo($"ASDASDSADS Item in spot1: {StatsManager.instance.playerInventorySpot1}");
        }
    }
*/
}