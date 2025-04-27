namespace BossMod.Components;

<<<<<<< HEAD:BossMod/Modules/Stormblood/Foray/DispelComponent.cs
public class DispelComponent(BossModule module, uint statusID, uint action = default) : Components.CastHint(module, action, "Prepare to dispel!")
=======
public class DispelHint(BossModule module, uint statusID, Enum? action = default) : CastHint(module, action, "Prepare to dispel!")
>>>>>>> merge:BossMod/Components/Dispel.cs
{
    private readonly List<Actor> Targets = [];

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var count = Targets.Count;
        if (count == 0)
            return;
        for (var i = 0; i < count; ++i)
            if (hints.FindEnemy(Targets[i]) is AIHints.Enemy target)
                target.ShouldBeDispelled = true;
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (Targets.Count > 0)
            hints.Add("Dispel!");
    }

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if (status.ID == statusID)
            Targets.Add(actor);
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if (status.ID == statusID)
            Targets.Remove(actor);
    }
}
