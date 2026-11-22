using System;

namespace MoreUpgradedCardPreviews.Compatibility;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MinGameVersionAttribute : Attribute
{
    public MinGameVersionAttribute(string version)
    {
        Version = version;
    }

    public string Version { get; }
}