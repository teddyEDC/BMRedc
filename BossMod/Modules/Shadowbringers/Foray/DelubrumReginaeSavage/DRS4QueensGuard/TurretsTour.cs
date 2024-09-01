namespace BossMod.Shadowbringers.Foray.DelubrumReginae.DRS4QueensGuard;

<<<<<<< HEAD:BossMod/Modules/Shadowbringers/Foray/DelubrumReginaeSavage/DRS4QueensGuard/TurretsTour.cs
class TurretsTour(BossModule module) : Components.GenericAOEs(module)
=======
class TurretsTour : Components.GenericAOEs
>>>>>>> merge:BossMod/Modules/Shadowbringers/Foray/DelubrumReginae/DRS4QueensGuard/TurretsTour.cs
{
    private readonly List<(Actor turret, AOEShapeRect shape)> _turrets = [];
    private readonly List<(Actor caster, AOEShapeRect shape, Angle rotation)> _casters = [];
    private DateTime _activation;

<<<<<<< HEAD:BossMod/Modules/Shadowbringers/Foray/DelubrumReginaeSavage/DRS4QueensGuard/TurretsTour.cs
    private static readonly AOEShapeRect _defaultShape = new(55, 3);

    public override void Update()
    {
        if (_turrets.Count == 0)
        {
            var turrets = Module.Enemies(OID.AutomaticTurret);
            foreach (var t in turrets)
            {
                var target = turrets.Exclude(t).InShape(_defaultShape, t).Closest(t.Position);
                var shape = target != null ? _defaultShape with { LengthFront = (target.Position - t.Position).Length() } : _defaultShape;
                _turrets.Add((t, shape));
            }
=======
    private static readonly AOEShapeRect _defaultShape = new(50, 3);

    public TurretsTour(BossModule module) : base(module)
    {
        var turrets = module.Enemies(OID.AutomaticTurret);
        foreach (var t in turrets)
        {
            var target = turrets.Exclude(t).InShape(_defaultShape, t).Closest(t.Position);
            var shape = target != null ? _defaultShape with { LengthFront = (target.Position - t.Position).Length() } : _defaultShape;
            _turrets.Add((t, shape));
>>>>>>> merge:BossMod/Modules/Shadowbringers/Foray/DelubrumReginae/DRS4QueensGuard/TurretsTour.cs
        }
    }

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        foreach (var t in _turrets)
            yield return new(t.shape, t.turret.Position, t.turret.Rotation, _activation);
        foreach (var c in _casters)
            yield return new(c.shape, c.caster.Position, c.rotation, Module.CastFinishAt(c.caster.CastInfo));
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
<<<<<<< HEAD:BossMod/Modules/Shadowbringers/Foray/DelubrumReginaeSavage/DRS4QueensGuard/TurretsTour.cs
        if ((AID)spell.Action.ID == AID.TurretsTourAOE1)
=======
        if ((AID)spell.Action.ID == AID.TurretsTourNormalAOE1)
>>>>>>> merge:BossMod/Modules/Shadowbringers/Foray/DelubrumReginae/DRS4QueensGuard/TurretsTour.cs
        {
            var toTarget = spell.LocXZ - caster.Position;
            _casters.Add((caster, new AOEShapeRect(toTarget.Length(), _defaultShape.HalfWidth), Angle.FromDirection(toTarget)));
            _activation = Module.CastFinishAt(spell);
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
<<<<<<< HEAD:BossMod/Modules/Shadowbringers/Foray/DelubrumReginaeSavage/DRS4QueensGuard/TurretsTour.cs
        if ((AID)spell.Action.ID == AID.TurretsTourAOE1)
=======
        if ((AID)spell.Action.ID == AID.TurretsTourNormalAOE1)
>>>>>>> merge:BossMod/Modules/Shadowbringers/Foray/DelubrumReginae/DRS4QueensGuard/TurretsTour.cs
            _casters.RemoveAll(c => c.caster == caster);
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
<<<<<<< HEAD:BossMod/Modules/Shadowbringers/Foray/DelubrumReginaeSavage/DRS4QueensGuard/TurretsTour.cs
        if ((AID)spell.Action.ID is AID.TurretsTourAOE2 or AID.TurretsTourAOE3)
=======
        if ((AID)spell.Action.ID is AID.TurretsTourNormalAOE2 or AID.TurretsTourNormalAOE3)
>>>>>>> merge:BossMod/Modules/Shadowbringers/Foray/DelubrumReginae/DRS4QueensGuard/TurretsTour.cs
        {
            _turrets.RemoveAll(t => t.turret == caster);
            ++NumCasts;
        }
    }
}
