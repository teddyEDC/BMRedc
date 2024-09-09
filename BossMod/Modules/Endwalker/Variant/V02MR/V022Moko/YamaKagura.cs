namespace BossMod.Endwalker.VariantCriterion.V02MR.V022Moko;

class YamaKagura(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.YamaKagura), 33, shape: new AOEShapeRect(40, 2.5f), kind: Kind.DirForward)
{
    private readonly List<(WPos, Angle)> data = [];
    private DateTime activation;
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        base.OnCastStarted(caster, spell);
        if ((AID)spell.Action.ID == AID.YamaKagura)
        {
            activation = Module.CastFinishAt(spell, 1);
            data.Add((caster.Position, spell.Rotation));
        }
    }

    public override void Update()
    {
        if (data.Count > 0 && WorldState.CurrentTime > activation)
        {
            data.Clear();
            ++NumCasts;
        }
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (Sources(slot, actor).Any() || activation > WorldState.CurrentTime) // 1s delay to wait for action effect
        {
            var length = Arena.Bounds.Radius * 2;
            var forbidden = new List<Func<WPos, float>>();
            foreach (var d in data)
                forbidden.Add(ShapeDistance.Rect(d.Item1, d.Item2, length, Distance - length, 2.5f));
            if (forbidden.Count > 0)
                hints.AddForbiddenZone(p => forbidden.Min(f => f(p)), activation.AddSeconds(-1));
        }
    }

    public override bool DestinationUnsafe(int slot, Actor actor, WPos pos) => (Module.FindComponent<GhastlyGrasp>()?.ActiveAOEs(slot, actor).Any(z => z.Shape.Check(pos, z.Origin, z.Rotation)) ?? false) || !Module.InBounds(pos);
}
