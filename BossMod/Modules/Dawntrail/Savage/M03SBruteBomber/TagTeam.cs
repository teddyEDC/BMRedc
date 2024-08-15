namespace BossMod.Dawntrail.Savage.M03SBruteBomber;

class TagTeamLariatCombo(BossModule module) : Components.GenericAOEs(module)
{
    public readonly List<AOEInstance> AOEs = [];
    private readonly Actor?[] _tetherSource = new Actor?[PartyState.MaxPartySize];
    private const string hint = "Go to correct spot!";
    private static readonly AOEShapeRect rect = new(70, 17);
    private static readonly AOEShapeCone cone = new(40, 135.Degrees());

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_tetherSource[slot] != null && AOEs.Count > 0)
        {
            List<Shape> shape1 = [];
            List<Shape> shape2 = [];
            List<Shape> difference = [];
            foreach (var aoe in AOEs.Where(x => x.Shape == rect))
            {
                var safe = _tetherSource[slot]?.Position.AlmostEqual(aoe.Origin, 25) ?? false;

                if (safe)
                    shape1.Add(new RectangleSE(aoe.Origin, aoe.Origin + rect.LengthFront * aoe.Rotation.ToDirection(), rect.HalfWidth));
                else if (!safe)
                    difference.Add(new RectangleSE(aoe.Origin, aoe.Origin + rect.LengthFront * aoe.Rotation.ToDirection(), rect.HalfWidth));
            }
            foreach (var aoe in AOEs.Where(x => x.Shape == cone))
                shape2.Add(new ConeHA(aoe.Origin, cone.Radius, aoe.Rotation, cone.HalfAngle));
            yield return new(new AOEShapeCustom(shape1, difference, shape2, true, shape2.Count > 0 ? OperandType.Intersection : OperandType.Union), Arena.Center, default, AOEs.FirstOrDefault().Activation, Colors.SafeFromAOE);
        }
        else
            foreach (var aoe in AOEs)
                yield return aoe;
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (ActiveAOEs(slot, actor).Any(c => c.Shape == rect))
            base.AddHints(slot, actor, hints);
        else if (ActiveAOEs(slot, actor).Any(c => !c.Check(actor.Position)))
            hints.Add(hint);
        else if (ActiveAOEs(slot, actor).Any(c => c.Check(actor.Position)))
            hints.Add(hint, false);
    }

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        if (tether.ID == (uint)TetherID.ChainDeathmatch && Raid.FindSlot(tether.Target) is var slot && slot >= 0)
            _tetherSource[slot] = source;
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.TagTeamLariatComboFirstRAOE or AID.TagTeamLariatComboFirstLAOE or AID.FusesOfFuryLariatComboFirstRAOE or AID.FusesOfFuryLariatComboFirstLAOE)
            AOEs.Add(new(rect, spell.LocXZ, spell.Rotation, Module.CastFinishAt(spell)));
        else if ((AID)spell.Action.ID == AID.FusesOfFuryMurderousMist)
            AOEs.Add(new(cone, caster.Position, spell.Rotation, Module.CastFinishAt(spell)));
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.TagTeamLariatComboFirstRAOE or AID.TagTeamLariatComboFirstLAOE or AID.TagTeamLariatComboSecondRAOE or AID.TagTeamLariatComboSecondLAOE
            or AID.FusesOfFuryLariatComboFirstRAOE or AID.FusesOfFuryLariatComboFirstLAOE or AID.FusesOfFuryLariatComboSecondRAOE or AID.FusesOfFuryLariatComboSecondLAOE)
        {
            ++NumCasts;
            Array.Fill(_tetherSource, null);
            var index = AOEs.FindIndex(aoe => aoe.Origin.AlmostEqual(spell.LocXZ, 1));
            if (index < 0)
            {
                ReportError($"Failed to find AOE for {spell.LocXZ}");
            }
            else if ((AID)spell.Action.ID is AID.TagTeamLariatComboFirstRAOE or AID.TagTeamLariatComboFirstLAOE or AID.FusesOfFuryLariatComboFirstRAOE or AID.FusesOfFuryLariatComboFirstLAOE)
            {
                ref var aoe = ref AOEs.Ref(index);
                aoe.Origin = Module.Center - (aoe.Origin - Module.Center);
                aoe.Rotation += 180.Degrees();
                aoe.Activation = WorldState.FutureTime(4.3f);
            }
            else
            {
                AOEs.RemoveAt(index);
            }
        }
        else if ((AID)spell.Action.ID == AID.FusesOfFuryMurderousMist)
            AOEs.RemoveAll(x => x.Shape == cone);
    }
}
