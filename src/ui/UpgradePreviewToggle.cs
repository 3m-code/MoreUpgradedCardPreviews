using System;

using Godot;

using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.UI;

public sealed class UpgradePreviewToggle
{
    private const string ToggleScenePath = "res://MoreUpgradedRewardsPreviews/assets/ui/upgrade_preview_toggle.tscn";
    private const string TickboxScriptPath = "res://src/ui/UpgradePreviewTickbox.cs";
    private const string GameTickboxScenePath = "res://scenes/ui/tickbox.tscn";
    private const string MegaLabelScriptPath = "res://addons/mega_text/MegaLabel.cs";
    private const string FontPath = "res://themes/kreon_bold_glyph_space_one.tres";
    
    private readonly Control _container;
    private readonly UpgradePreviewTickbox _tickbox;
    private readonly Action<bool> _applyState;
    
    private bool _isSyncing;
    private bool _disposed;

    private UpgradePreviewToggle(Control container, UpgradePreviewTickbox tickbox, Action<bool> applyState)
    {
        _container = container;
        _tickbox = tickbox;
        _applyState = applyState;
    }
    
    public Control Container => _container;
    public UpgradePreviewTickbox Tickbox => _tickbox;

    public static UpgradePreviewToggle? TryCreate(Node parent, Action<bool> applyState)
    {
        try
        {
            if (parent.GetNodeOrNull<NTickbox>("%Upgrades") != null) return null;
            if (parent.GetNodeOrNull<Control>("UpgradePreviewToggleContainer") != null) return null;

            var container = InstantiateContainer();
            if (container == null) return null;

            var upgradesNode = container.GetNode<HBoxContainer>("MarginContainer/Upgrades");
            var labelNode = container.GetNode<Label>("MarginContainer/Upgrades/ViewUpgradesLabel");

            var tickbox = SetupUpgradePreviewTickbox(upgradesNode);
            if (tickbox == null)
            {
                container.QueueFree();
                return null;
            }

            if (!SetupGameTickboxVisuals(container, tickbox))
            {
                container.QueueFree();
                return null;
            }

            if (!SetupMegaLabel(labelNode))
            {
                container.QueueFree();
                return null;
            }

            parent.AddChild(container);

            var toggle = new UpgradePreviewToggle(container, tickbox, applyState);
            parent.TreeExiting += toggle.Dispose;
            toggle.Initialize();
            return toggle;
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to create UpgradePreviewToggle: {ex}");
            return null;
        }
    }

    private static Control? InstantiateContainer()
    {
        var scene = PreloadManager.Cache.GetScene(ToggleScenePath);
        if (scene == null)
        {
            Main.Logger.Error($"Could not load toggle scene: {ToggleScenePath}");
            return null;
        }

        var container = scene.Instantiate<Control>();
        if (container == null)
        {
            Main.Logger.Error("Could not instantiate upgrade preview toggle.");
            return null;
        }

        return container;
    }

    private static UpgradePreviewTickbox? SetupUpgradePreviewTickbox(HBoxContainer upgradesNode)
    {
        var tickboxScript = GD.Load<CSharpScript>(TickboxScriptPath);
        if (tickboxScript == null)
        {
            Main.Logger.Error($"Could not load tickbox script: {TickboxScriptPath}");
            return null;
        }

        var tickboxId = upgradesNode.GetInstanceId();
        upgradesNode.SetScript(tickboxScript);
        
        var tickbox = GodotObject.InstanceFromId(tickboxId) as UpgradePreviewTickbox;
        if (tickbox == null)
        {
            Main.Logger.Error("Failed to obtain UpgradePreviewTickbox.");
            return null;
        }

        //Main.Logger.Info($"UpgradePreviewTickbox runtime type: {tickbox.GetType().FullName}");
        return tickbox;
    }

    private static bool SetupGameTickboxVisuals(Control container, UpgradePreviewTickbox tickbox)
    {
        var gameTickboxScene = PreloadManager.Cache.GetScene(GameTickboxScenePath);
        if (gameTickboxScene == null)
        {
            Main.Logger.Error($"Could not load game tickbox scene: {GameTickboxScenePath}");
            return false;
        }

        var tickboxVisuals = gameTickboxScene.Instantiate<Control>();
        if (tickboxVisuals == null)
        {
            Main.Logger.Error("Could not instantiate game TickboxVisuals.");
            return false;
        }

        tickboxVisuals.Name = "TickboxVisuals";
        tickbox.AddChild(tickboxVisuals);
        tickbox.MoveChild(tickboxVisuals, 0);
        tickboxVisuals.UniqueNameInOwner = true;
        tickboxVisuals.Owner = container;

        return true;
    }

    private static bool SetupMegaLabel(Label labelNode) 
    {
        var megaLabelScript = GD.Load<CSharpScript>(MegaLabelScriptPath);
        if (megaLabelScript == null)
        {
            Main.Logger.Error($"Could not load MegaLabel script: {MegaLabelScriptPath}");
            return false;
        }

        var labelId = labelNode.GetInstanceId();

        labelNode.SetScript(megaLabelScript);

        var label = GodotObject.InstanceFromId(labelId) as MegaLabel;

        if (label == null)
        {
            Main.Logger.Error("Failed to obtain MegaLabel.");
            return false;
        }

        var font = GD.Load<Font>(FontPath);
        if (font == null)
        {
            Main.Logger.Error($"Could not load font: {FontPath}");
            return false;
        }

        label.AddThemeFontOverride(ThemeConstants.Label.Font, font);
        label.AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, 27);

        label.AutoSizeEnabled = false;
        label.MinFontSize = 28;
        label.MaxFontSize = 28;
        
        label.SetTextAutoSize(
            new LocString(
                "card_selection",
                "VIEW_UPGRADES"
            ).GetFormattedText());

        return true;
    }

    private void Initialize()
    {
        _tickbox.Toggled += OnToggled;

        UpgradePreviewManager.Instance.OnToggleStateChanged += OnManagerStateChanged;
        UpgradePreviewManager.Instance.SetShowingUpgrades(false);

        SyncFromManager();
    }

    private void OnToggled(NTickbox tickbox)
    {
        if (_isSyncing || _disposed) return;
        
        UpgradePreviewManager.Instance.SetShowingUpgrades(tickbox.IsTicked);
    }

    private void OnManagerStateChanged(bool showingUpgrades)
    {
        if (_disposed) return;

        SyncOurTickbox(showingUpgrades);
        ApplyState(showingUpgrades);
    }

    private void SyncFromManager()
    {
        var showingUpgrades = UpgradePreviewManager.Instance.IsShowingUpgrades;

        SyncOurTickbox(showingUpgrades);
        ApplyState(showingUpgrades);
    }

    private void SyncOurTickbox(bool showingUpgrades)
    {
        _isSyncing = true;

        try
        {
            if (_tickbox.IsTicked != showingUpgrades) _tickbox.IsTicked = showingUpgrades;
        }
        finally
        {
            _isSyncing = false;
        }
    }

    private void ApplyState(bool showingUpgrades)
    {
        try
        {
            _applyState(showingUpgrades);
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to apply upgrade preview state: {ex}");
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _tickbox.Toggled -= OnToggled;
        UpgradePreviewManager.Instance.OnToggleStateChanged -= OnManagerStateChanged;
        
        if (GodotObject.IsInstanceValid(_container))
        {
            var parent = _container.GetParent();
            if (parent != null) parent.RemoveChild(_container);

            _container.QueueFree();
        }
    }
}