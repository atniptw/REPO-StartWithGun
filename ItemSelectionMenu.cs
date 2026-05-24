using System.Linq;
using MenuLib;
using MenuLib.MonoBehaviors;

namespace StartWithGun;

internal static class ItemSelectionMenu
{
    private static readonly (string Category, (string AssetName, string DisplayName)[] Items)[] Categories =
    [
        ("Guns", [
            ("Item Gun Handgun", "Handgun"),
            ("Item Gun Shotgun", "Shotgun"),
            ("Item Gun Tranq", "Tranq Gun"),
        ]),
        ("Grenades", [
            ("Item Grenade Duct Taped", "Duct Taped Grenade"),
            ("Item Grenade Explosive", "Grenade"),
            ("Item Grenade Human", "Human Grenade"),
            ("Item Grenade Shockwave", "Shockwave Grenade"),
            ("Item Grenade Stun", "Stun Grenade"),
        ]),
        ("Melee", [
            ("Item Melee Baseball Bat", "Baseball Bat"),
            ("Item Melee Frying Pan", "Frying Pan"),
            ("Item Melee Inflatable Hammer", "Inflatable Hammer"),
            ("Item Melee Sledge Hammer", "Sledge Hammer"),
            ("Item Melee Sword", "Sword"),
        ]),
        ("Mines", [
            ("Item Mine Explosive", "Explosive Mine"),
            ("Item Mine Shockwave", "Shockwave Mine"),
            ("Item Mine Stun", "Stun Mine"),
        ]),
        ("Drones", [
            ("Item Drone Battery", "Recharge Drone"),
            ("Item Drone Feather", "Feather Drone"),
            ("Item Drone Indestructible", "Indestructible Drone"),
            ("Item Drone Torque", "Roll Drone"),
            ("Item Drone Zero Gravity", "Zero Gravity Drone"),
        ]),
        ("Health", [
            ("Item Health Pack Small", "Small Health Pack"),
            ("Item Health Pack Medium", "Medium Health Pack"),
            ("Item Health Pack Large", "Large Health Pack"),
        ]),
        ("Carts", [
            ("Item Cart Medium", "C.A.R.T."),
            ("Item Cart Small", "Pocket C.A.R.T."),
        ]),
        ("Upgrades", [
            ("Item Upgrade Map Player Count", "Map Player Count"),
            ("Item Upgrade Player Energy", "Stamina"),
            ("Item Upgrade Player Extra Jump", "Extra Jump"),
            ("Item Upgrade Player Grab Range", "Grab Range"),
            ("Item Upgrade Player Grab Strength", "Grab Strength"),
            ("Item Upgrade Player Health", "Health"),
            ("Item Upgrade Player Sprint Speed", "Sprint Speed"),
            ("Item Upgrade Player Tumble Launch", "Tumble Launch"),
        ]),
        ("Misc", [
            ("Item Orb Zero Gravity", "Zero Gravity Orb"),
            ("Item Power Crystal", "Energy Crystal"),
            ("Item Extraction Tracker", "Extraction Tracker"),
            ("Item Valuable Tracker", "Valuable Tracker"),
            ("Item Rubber Duck", "Rubber Duck"),
        ]),
    ];

    internal static void Register()
    {
        MenuAPI.AddElementToMainMenu(parent =>
            MenuAPI.CreateREPOButton("Starting Item", OpenCategoryPage, parent));
        MenuAPI.AddElementToEscapeMenu(parent =>
            MenuAPI.CreateREPOButton("Starting Item", OpenCategoryPage, parent));
    }

    private static void OpenCategoryPage()
    {
        var page = MenuAPI.CreateREPOPopupPage("Starting Item", REPOPopupPage.PresetSide.Left, false, false, 0f);
        foreach (var (category, _) in Categories)
        {
            var cat = category;
            page.AddElementToScrollView(parent =>
            {
                var btn = MenuAPI.CreateREPOButton(cat, () => OpenItemPage(cat), parent);
                return btn.rectTransform;
            });
        }
        page.OpenPage(false);
    }

    private static void OpenItemPage(string category)
    {
        var items = Categories.First(c => c.Category == category).Items;
        var currentValue = StartWithGun.gun.Value;

        var page = MenuAPI.CreateREPOPopupPage(category, REPOPopupPage.PresetSide.Right, false, false, 0f);
        foreach (var (assetName, displayName) in items)
        {
            var asset = assetName;
            var label = assetName == currentValue ? $"> {displayName}" : displayName;
            page.AddElementToScrollView(parent =>
            {
                var btn = MenuAPI.CreateREPOButton(label, () =>
                {
                    StartWithGun.gun.Value = asset;
                    MenuAPI.CloseAllPagesAddedOnTop();
                }, parent);
                return btn.rectTransform;
            });
        }
        page.OpenPage(true);
    }
}
