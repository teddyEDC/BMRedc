using static BossMod.Shadowbringers.Foray.CastrumLacusLitore.CLL1Brionac4thLegionHelldiver.CLL1Brionac4thLegionHelldiver;

namespace BossMod.Shadowbringers.Foray.CastrumLacusLitore.CLL1Brionac4thLegionHelldiver;

class WildCharges(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = new(6);

    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (ArenaBottom.Contains(actor.Position - ArenaCenterBottom))
            return CollectionsMarshal.AsSpan(_aoes);
        else
            return [];
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        var halfWidth = spell.Action.ID switch
        {
            (uint)AID.DiveFormation1 or (uint)AID.LinearDive => 3f,
            (uint)AID.DiveFormation2 => 5f,
            _ => default
        };
        if (halfWidth != default)
        {
            var dir = spell.LocXZ - caster.Position;
            _aoes.Add(new(new AOEShapeRect(dir.Length(), 4f, InvertForbiddenZone: true), WPos.ClampToGrid(caster.Position), Angle.FromDirection(dir), Module.CastFinishAt(spell), Colors.SafeFromAOE));
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count != 0 && spell.Action.ID is (uint)AID.DiveFormation1 or (uint)AID.DiveFormation2 or (uint)AID.LinearDive)
            _aoes.RemoveAt(0);
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var count = _aoes.Count;
        if (count != 0)
        {
            var forbidden = new Func<WPos, float>[count];
            for (var i = 0; i < count; ++i)
            {
                var aoe = _aoes[i];
                forbidden[i] = aoe.Shape.Distance(aoe.Origin, aoe.Rotation);
            }
            hints.AddForbiddenZone(ShapeDistance.Intersection(forbidden), _aoes[0].Activation);
        }
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        var count = _aoes.Count;
        if (count == 0)
            return;

        var risky = true;
        for (var i = 0; i < count; ++i)
        {
            var aoe = _aoes[i];
            if (aoe.Check(actor.Position))
            {
                risky = false;
                break;
            }
        }
        hints.Add("Share damage inside wildcharge!", risky);
    }
}
