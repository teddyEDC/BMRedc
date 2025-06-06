﻿using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

namespace BossMod.Autorotation;

public sealed class UIRotationWindow : UIWindow
{
    private readonly RotationModuleManager _mgr;
    private readonly ActionManagerEx _amex;
    private readonly AutorotationConfig _config = Service.Config.Get<AutorotationConfig>();
    private readonly EventSubscriptions _subscriptions;

    public UIRotationWindow(RotationModuleManager mgr, ActionManagerEx amex, Action openConfig) : base("Autorotation", false, new(400, 400), ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoFocusOnAppearing)
    {
        _mgr = mgr;
        _amex = amex;
        _subscriptions = new
        (
            _config.Modified.ExecuteAndSubscribe(() => IsOpen = _config.ShowUI)
        );
        RespectCloseHotkey = false;
        TitleBarButtons.Add(new() { Icon = FontAwesomeIcon.Cog, IconOffset = new(1), Click = _ => openConfig() });
    }

    protected override void Dispose(bool disposing)
    {
        _subscriptions.Dispose();
        base.Dispose(disposing);
    }

    public void SetVisible(bool vis)
    {
        if (_config.ShowUI != vis)
        {
            _config.ShowUI = vis;
            _config.Modified.Fire();
        }
    }

    public override void PreOpenCheck()
    {
        DrawPositional();
    }

    public override bool DrawConditions() => _mgr.WorldState.Party.Player() != null;

    public override void Draw()
    {
        var player = _mgr.Player;
        if (player == null)
            return;

        DrawRotationSelector(_mgr);

        var activeModule = _mgr.Bossmods.ActiveModule;
        if (activeModule != null)
        {
            ImGui.TextUnformatted($"CD Plan:");

            if (activeModule.Info?.PlanLevel > 0)
            {
                ImGui.SameLine();
                var plans = _mgr.Database.Plans.GetPlans(activeModule.GetType(), player.Class);
                var newSel = UIPlanDatabaseEditor.DrawPlanCombo(plans, plans.SelectedIndex, "");
                if (newSel != plans.SelectedIndex)
                {
                    plans.SelectedIndex = newSel;
                    _mgr.Database.Plans.ModifyManifest(activeModule.GetType(), player.Class);
                }

                ImGui.SameLine();
                if (ImGui.Button(plans.SelectedIndex >= 0 ? "Edit" : "New"))
                {
                    if (plans.SelectedIndex < 0)
                    {
                        var plan = new Plan($"New {plans.Plans.Count + 1}", activeModule.GetType()) { Guid = Guid.NewGuid().ToString(), Class = player.Class, Level = activeModule.Info.PlanLevel };
                        plans.SelectedIndex = plans.Plans.Count;
                        _mgr.Database.Plans.ModifyPlan(null, plan);
                    }
                    UIPlanDatabaseEditor.StartPlanEditor(_mgr.Database.Plans, plans.Plans[plans.SelectedIndex], activeModule.StateMachine);
                }

                if (newSel >= 0 && _mgr.Preset != null)
                {
                    ImGui.SameLine();
                    using var style = ImRaii.PushColor(ImGuiCol.Text, Colors.TextColor2);
                    UIMisc.HelpMarker(() => "You have a preset activated, which fully overrides the CD plan!", FontAwesomeIcon.ExclamationTriangle);
                }
            }
        }

        // TODO: more fancy action history/queue...
        ImGui.TextUnformatted($"Modules: {_mgr}");
        if (_mgr.Preset?.Modules.Any(m => m.TransientSettings.Count > 0) ?? false)
        {
            ImGui.SameLine();
            using (ImRaii.PushColor(ImGuiCol.Text, 0xff00ff00))
                UIMisc.IconText(FontAwesomeIcon.BoltLightning, "(4)");
            if (ImGui.IsItemHovered())
            {
                using var tooltip = ImRaii.Tooltip();
                ImGui.TextUnformatted("Transient strategies:");
                foreach (var m in _mgr.Preset.Modules.Where(m => m.TransientSettings.Count > 0))
                {
                    ImGui.TextUnformatted($"> {m.Type.FullName}");
                    using var indent = ImRaii.PushIndent();
                    foreach (var s in m.TransientSettings)
                    {
                        var track = m.Definition.Configs[s.Track];
                        ImGui.TextUnformatted($"{track.InternalName} = {track.Options[s.Value.Option].InternalName}");
                    }
                }
            }
        }

        ImGui.TextUnformatted($"GCD={_mgr.WorldState.Client.Cooldowns[ActionDefinitions.GCDGroup].Remaining:f3}, AnimLock={_amex.EffectiveAnimationLock:f3}+{_amex.AnimationLockDelayEstimate:f3}, Combo={_amex.ComboTimeLeft:f3}, RBIn={_mgr.Bossmods.RaidCooldowns.NextDamageBuffIn():f3}");
        foreach (var a in _mgr.Hints.ActionsToExecute.Entries)
        {
            ImGui.TextUnformatted($"> {a.Action} ({a.Priority:f2}) @ ({a.Target?.Name ?? "<none>"})");
        }
    }

    public override void OnClose() => SetVisible(false);

    public static bool DrawRotationSelector(RotationModuleManager mgr)
    {
        var modified = false;
        if (mgr.Player == null)
            return modified;

        ImGui.TextUnformatted("Presets:");

        ImGui.SameLine();

        using (ImRaii.PushColor(ImGuiCol.Button, Colors.ButtonPushColor1, mgr.Preset == RotationModuleManager.ForceDisable))
        using (ImRaii.PushColor(ImGuiCol.ButtonHovered, Colors.ButtonPushColor3, mgr.Preset == RotationModuleManager.ForceDisable))
        using (ImRaii.PushColor(ImGuiCol.ButtonActive, Colors.ButtonPushColor4, mgr.Preset == RotationModuleManager.ForceDisable))
        {
            if (ImGui.Button("Disabled"))
            {
                mgr.Preset = mgr.Preset == RotationModuleManager.ForceDisable ? null : RotationModuleManager.ForceDisable;
                modified |= true;
            }
        }

        foreach (var p in mgr.Database.Presets.PresetsForClass(mgr.Player.Class))
        {
            ImGui.SameLine();
            using var col = ImRaii.PushColor(ImGuiCol.Button, Colors.ButtonPushColor2, mgr.Preset == p);
            using var colHovered = ImRaii.PushColor(ImGuiCol.ButtonHovered, Colors.ButtonPushColor5, mgr.Preset == p);
            using var colActive = ImRaii.PushColor(ImGuiCol.ButtonActive, Colors.ButtonPushColor6, mgr.Preset == p);
            if (ImGui.Button(p.Name))
            {
                mgr.Preset = mgr.Preset == p ? null : p;
                modified |= true;
            }
        }

        return modified;
    }

    private void DrawPositional()
    {
        var pos = _mgr.Hints.RecommendedPositional;
        if (_config.ShowPositionals && pos.Target != null && !pos.Target.Omnidirectional)
        {
            var color = PositionalColor(pos.Imminent, pos.Correct);
            switch (pos.Pos)
            {
                case Positional.Flank:
                    Camera.Instance?.DrawWorldCone(pos.Target.PosRot.XYZ(), pos.Target.HitboxRadius + 3.5f, pos.Target.Rotation + 90.Degrees(), 45.Degrees(), color);
                    Camera.Instance?.DrawWorldCone(pos.Target.PosRot.XYZ(), pos.Target.HitboxRadius + 3.5f, pos.Target.Rotation - 90.Degrees(), 45.Degrees(), color);
                    break;
                case Positional.Rear:
                    Camera.Instance?.DrawWorldCone(pos.Target.PosRot.XYZ(), pos.Target.HitboxRadius + 3.5f, pos.Target.Rotation + 180.Degrees(), 45.Degrees(), color);
                    break;
            }
        }
    }

    private static uint PositionalColor(bool imminent, bool correct) => imminent
        ? (correct ? Colors.PositionalColor1 : Colors.PositionalColor2)
        : (correct ? Colors.PositionalColor3 : Colors.PositionalColor4);
}
