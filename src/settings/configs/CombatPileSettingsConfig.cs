using System;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class CombatPileSettingsConfig
{
    private const string DataKey = "combat_pile_settings";
    private const string FileName = "combat_pile_settings.json";
    public static readonly CombatPileSettingsConfig Instance = new();

    private CombatPileSettingsConfig()
    {
        DrawPileBinding = new ModSettingsValueBinding<CombatPileSettings, bool>(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.DrawPilePreviewEnabled, SetDrawPilePreviewEnabled
        );

        DiscardPileBinding = new ModSettingsValueBinding<CombatPileSettings, bool>(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.DiscardPilePreviewEnabled, SetDiscardPilePreviewEnabled
        );

        ExhaustPileBinding = new ModSettingsValueBinding<CombatPileSettings, bool>(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.ExhaustPilePreviewEnabled, SetExhaustPilePreviewEnabled
        );
    }

    public ModSettingsValueBinding<CombatPileSettings, bool> DrawPileBinding { get; }

    public ModSettingsValueBinding<CombatPileSettings, bool> DiscardPileBinding { get; }

    public ModSettingsValueBinding<CombatPileSettings, bool> ExhaustPileBinding { get; }

    public event Action<bool>? DrawPilePreviewEnabledChanged;
    public event Action<bool>? DiscardPilePreviewEnabledChanged;
    public event Action<bool>? ExhaustPilePreviewEnabledChanged;

    public void Register()
    {
        RegisterData();
    }

    public bool IsUpgradePreviewEnabled(PileType pileType)
    {
        return pileType switch
        {
            PileType.Draw => DrawPileBinding.Read(),
            PileType.Discard => DiscardPileBinding.Read(),
            PileType.Exhaust => ExhaustPileBinding.Read(),
            _ => false
        };
    }

    public void SubscribeToUpgradePreviewChanges(PileType pileType, Action<bool> handler)
    {
        switch (pileType)
        {
            case PileType.Draw:
                DrawPilePreviewEnabledChanged += handler;
                break;

            case PileType.Discard:
                DiscardPilePreviewEnabledChanged += handler;
                break;

            case PileType.Exhaust:
                ExhaustPilePreviewEnabledChanged += handler;
                break;
        }
    }

    public void UnsubscribeFromUpgradePreviewChanges(PileType pileType, Action<bool> handler)
    {
        switch (pileType)
        {
            case PileType.Draw:
                DrawPilePreviewEnabledChanged -= handler;
                break;

            case PileType.Discard:
                DiscardPilePreviewEnabledChanged -= handler;
                break;

            case PileType.Exhaust:
                ExhaustPilePreviewEnabledChanged -= handler;
                break;
        }
    }

    private void RegisterData()
    {
        using (RitsuLibFramework.BeginModDataRegistration(ModInfo.Id))
        {
            var store = RitsuLibFramework.GetDataStore(ModInfo.Id);

            store.Register(
                DataKey,
                FileName,
                SaveScope.Global,
                () => new CombatPileSettings(),
                true
            );
        }
    }

    private void SetDrawPilePreviewEnabled(CombatPileSettings settings, bool value)
    {
        if (settings.DrawPilePreviewEnabled == value) return;

        settings.DrawPilePreviewEnabled = value;
        DrawPilePreviewEnabledChanged?.Invoke(value);
    }

    private void SetDiscardPilePreviewEnabled(CombatPileSettings settings, bool value)
    {
        if (settings.DiscardPilePreviewEnabled == value) return;

        settings.DiscardPilePreviewEnabled = value;
        DiscardPilePreviewEnabledChanged?.Invoke(value);
    }

    private void SetExhaustPilePreviewEnabled(CombatPileSettings settings, bool value)
    {
        if (settings.ExhaustPilePreviewEnabled == value) return;

        settings.ExhaustPilePreviewEnabled = value;
        ExhaustPilePreviewEnabledChanged?.Invoke(value);
    }
}