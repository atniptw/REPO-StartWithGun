using System.Collections.Generic;
using Xunit;

namespace StartWithGun.Tests;

public class GrantItemTests
{
    [Fact]
    public void WhenKeyMissing_GrantsItem()
    {
        var purchased = new Dictionary<string, int>();
        var total = new Dictionary<string, int>();

        var granted = ItemGranter.TryGrantItem(purchased, total, "Item Gun Handgun");

        Assert.True(granted);
        Assert.Equal(1, purchased["Item Gun Handgun"]);
    }

    [Fact]
    public void WhenCountIsZero_GrantsItem()
    {
        var purchased = new Dictionary<string, int> { ["Item Gun Handgun"] = 0 };
        var total = new Dictionary<string, int>();

        var granted = ItemGranter.TryGrantItem(purchased, total, "Item Gun Handgun");

        Assert.True(granted);
        Assert.Equal(1, purchased["Item Gun Handgun"]);
    }

    [Fact]
    public void WhenAlreadyOwned_DoesNotGrant()
    {
        var purchased = new Dictionary<string, int> { ["Item Gun Handgun"] = 2 };
        var total = new Dictionary<string, int> { ["Item Gun Handgun"] = 2 };

        var granted = ItemGranter.TryGrantItem(purchased, total, "Item Gun Handgun");

        Assert.False(granted);
        Assert.Equal(2, purchased["Item Gun Handgun"]);
    }

    [Fact]
    public void WhenGranted_SetsTotalToOneWhenKeyMissing()
    {
        var purchased = new Dictionary<string, int>();
        var total = new Dictionary<string, int>();

        ItemGranter.TryGrantItem(purchased, total, "Item Gun Handgun");

        Assert.Equal(1, total["Item Gun Handgun"]);
    }

    [Fact]
    public void WhenGranted_IncrementsTotalWhenKeyExists()
    {
        var purchased = new Dictionary<string, int>();
        var total = new Dictionary<string, int> { ["Item Gun Handgun"] = 3 };

        ItemGranter.TryGrantItem(purchased, total, "Item Gun Handgun");

        Assert.Equal(4, total["Item Gun Handgun"]);
    }
}
