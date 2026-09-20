using System;

using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace MoreUpgradedRewardsPreviews.Core;

public abstract class SingleUpgradePreviewSettingsConfig<TSettings> where TSettings : class, new()
{
    private readonly Func<TSettings, bool> _getEnabled;
    private readonly Action<TSettings, bool> _setEnabled;
    private readonly Func<TSettings> _defaultFactory;

    private readonly ModSettingsValueBinding<TSettings, bool> _upgradePreviewBinding;

    protected SingleUpgradePreviewSettingsConfig(
        string pageId,
        string dataKey,
        string fileName,
        string pageTitle,
        string pageDescription,
        string settingLabel,
        Func<TSettings, bool> getEnabled,
        Action<TSettings, bool> setEnabled,
        Func<TSettings> defaultFactory)
    {
        PageId = pageId;
        DataKey = dataKey;
        FileName = fileName;
        PageTitle = pageTitle;
        PageDescription = pageDescription;
        SettingLabel = settingLabel;

        _getEnabled = getEnabled;
        _setEnabled = setEnabled;
        _defaultFactory = defaultFactory;

        _upgradePreviewBinding = new(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            _getEnabled,
            SetUpgradePreviewEnabled);
    }

    private string PageId { get; }

    private string DataKey { get; }

    private string FileName { get; }

    private string PageTitle { get; }

    private string PageDescription { get; }

    private string SettingLabel { get; }

    public event Action<bool>? UpgradePreviewEnabledChanged;

    public bool IsUpgradePreviewEnabled()
    {
        return _upgradePreviewBinding.Read();
    }

    public void SubscribeToUpgradePreviewChanges(Action<bool> handler)
    {
        UpgradePreviewEnabledChanged += handler;
    }

    public void UnsubscribeFromUpgradePreviewChanges(Action<bool> handler)
    {
        UpgradePreviewEnabledChanged -= handler;
    }

    public void Register()
    {
        RegisterData();
        RegisterSettingsPage();
    }

    protected ModSettingsValueBinding<TSettings, bool> UpgradePreviewBinding => _upgradePreviewBinding;

    private void RegisterData()
    {
        using (RitsuLibFramework.BeginModDataRegistration(ModInfo.Id))
        {
            var store = RitsuLibFramework.GetDataStore(ModInfo.Id);

            store.Register(
                key: DataKey,
                fileName: FileName,
                scope: SaveScope.Global,
                defaultFactory: _defaultFactory,
                autoCreateIfMissing: true);
        }
    }

    private void RegisterSettingsPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal(PageTitle))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal(PageDescription))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("upgrade_preview", section => section.WithTitle(ModSettingsText.Literal("Upgrade Preview"))
                            .AddToggle("enabled", ModSettingsText.Literal(SettingLabel), _upgradePreviewBinding));
            },
            PageId);
    }

    private void SetUpgradePreviewEnabled(TSettings settings, bool value)
    {
        if (_getEnabled(settings) == value) return;

        _setEnabled(settings, value);
        UpgradePreviewEnabledChanged?.Invoke(value);
    }
}