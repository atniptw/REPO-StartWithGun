using System.Collections.Generic;

namespace StartWithGun;

internal static class ItemGranter
{
    internal static bool TryGrantItem(
        Dictionary<string, int> purchased,
        Dictionary<string, int> purchasedTotal,
        string itemAssetName)
    {
        if (!purchased.TryGetValue(itemAssetName, out var count) || count == 0)
        {
            purchased[itemAssetName] = 1;
            purchasedTotal.TryGetValue(itemAssetName, out var total);
            purchasedTotal[itemAssetName] = total + 1;
            return true;
        }
        return false;
    }
}
