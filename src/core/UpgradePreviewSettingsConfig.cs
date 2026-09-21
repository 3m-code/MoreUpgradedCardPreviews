using System;

using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace MoreUpgradedCardPreviews.Core;

public abstract class SingleUpgradePreviewSettingsConfig<TSettings> where TSettings : class, new()
{
    private readonly string _dataKey;
    private readonly string _fileName;

    private readonly Func<TSettings, bool> _getEnabled;
    private readonly Action<TSettings, bool> _setEnabled;
    private readonly Func<TSettings> _defaultFactory;

    private readonly ModSettingsValueBinding<TSettings, bool> _upgradePreviewBinding;

    protected SingleUpgradePreviewSettingsConfig(
        string dataKey,
        string fileName,
        Func<TSettings, bool> getEnabled,
        Action<TSettings, bool> setEnabled,
        Func<TSettings> defaultFactory)
    {
        _dataKey = dataKey;
        _fileName = fileName;

        _getEnabled = getEnabled;
        _setEnabled = setEnabled;
        _defaultFactory = defaultFactory;

        _upgradePreviewBinding = new(
            ModInfo.Id,
            _dataKey,
            SaveScope.Global,
            _getEnabled,
            SetUpgradePreviewEnabled
            );
    }

    public event Action<bool>? UpgradePreviewEnabledChanged;

    public ModSettingsValueBinding<TSettings, bool> UpgradePreviewBinding => _upgradePreviewBinding;

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
    }

    private void RegisterData()
    {
        using (RitsuLibFramework.BeginModDataRegistration(ModInfo.Id))
        {
            var store = RitsuLibFramework.GetDataStore(ModInfo.Id);

            store.Register(
                key: _dataKey,
                fileName: _fileName,
                scope: SaveScope.Global,
                defaultFactory: _defaultFactory,
                autoCreateIfMissing: true);
        }
    }

    private void SetUpgradePreviewEnabled(TSettings settings, bool value)
    {
        if (_getEnabled(settings) == value) return;

        _setEnabled(settings, value);
        UpgradePreviewEnabledChanged?.Invoke(value);
    }
}