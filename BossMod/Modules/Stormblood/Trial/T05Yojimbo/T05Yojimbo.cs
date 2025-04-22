namespace BossMod.Stormblood.Trial.T05Yojimbo;

class MettaGiri(BossModule module) : Components.RaidwideCast(module, (uint)AID.MettaGiri);
class Yukikaze2(BossModule module) : Components.SimpleAOEs(module, (uint)AID.Yukikaze2, new AOEShapeRect(44.5f, 2));
class TinySong(BossModule module) : Components.StackTogether(module, 55, 5f, 1f);
class BitterEnd2(BossModule module) : Components.Cleave(module, (uint)AID.BitterEnd2, new AOEShapeCone(9.8f, 45f.Degrees()));
class AmeNoMurakumo(BossModule module) : Components.RaidwideCast(module, (uint)AID.AmeNoMurakumo);
class Masamune(BossModule module) : Components.ChargeAOEs(module, (uint)AID.Masamune, 4);
class ZanmaZanmai(BossModule module) : Components.RaidwideCast(module, (uint)AID.ZanmaZanmai, "Raidwide drop to 1 hp");

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "The Combat Reborn Team, Chuggalo", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 595, NameID = 6089)]
public class T05Yojimbo(WorldState ws, Actor primary) : BossModule(ws, primary, new(100f, 100f), new ArenaBoundsSquare(20f))
{

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor);
        Arena.Actors(Enemies((uint)OID.Embodiment));
    }
}
