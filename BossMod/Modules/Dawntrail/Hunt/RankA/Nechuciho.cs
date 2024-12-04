namespace BossMod.Dawntrail.Hunt.RankA.Nechuciho;

public enum OID : uint
{
    Boss = 0x452C // R3.42
}

public enum AID : uint
{
    AutoAttack = 872, // Boss->player, no cast, single-target
    CastWordOfTheWood = 39872, // Boss->self, 5.0s cast, range 30 180.000-degree cone
    WordOfTheWood1 = 39785, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood2 = 39786, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood3 = 39787, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood4 = 39788, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood5 = 42164, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood6 = 42165, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood7 = 42166, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood8 = 42167, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood9 = 42168, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood10 = 42169, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood11 = 42170, // Boss->self, no cast, range 30 180.000-degree cone
    WordOfTheWood12 = 42171, // Boss->self, no cast, range 30 180.000-degree cone
    SentinelRoar = 39491, // Boss->self, 5.0s cast, range 40 circle
    Level5DeathSentence = 39492, // Boss->self, 5.0s cast, range 30 circle
    WhisperOfTheWood1 = 39493, // Boss->self, 3.0s cast, single-target
    WhisperOfTheWood2 = 39494, // Boss->self, 3.0s cast, single-target
    WhisperOfTheWood3 = 39495, // Boss->self, 3.0s cast, single-target
}

class Level5DeathSentence(BossModule module) : Components.CastInterruptHint(module, ActionID.MakeSpell(AID.Level5DeathSentence), true, false, "Applies Doom!");
class SentinelRoar(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.SentinelRoar));
class WordOfTheWood(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.CastWordOfTheWood), new AOEShapeCone(30, 90.Degrees()));

class WhispersOfTheWood(BossModule module) : Components.GenericRotatingAOE(module)
{
    private static readonly AOEShapeCone _shape = new(30, 90.Degrees());
    private static readonly Dictionary<string, Angle[]> _angleSequences = new()
        {
            { "1", new[] { 0.Degrees(), -90.Degrees(), 90.Degrees(), 180.Degrees() } },
            { "2", new[] { 90.Degrees(), 0.Degrees(), 180.Degrees(), -90.Degrees() } },
            { "3", new[] { 180.Degrees(), 90.Degrees(), -90.Degrees(), 0.Degrees() } }
        };

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if (caster != Module.PrimaryActor) return;

        var aidName = ((AID)spell.Action.ID).ToString();
        if (!aidName.StartsWith("WhisperOfTheWood")) return;

        var suffix = aidName.Substring("WhisperOfTheWood".Length);
        if (!_angleSequences.TryGetValue(suffix, out var angles)) return;

        var activation = Module.CastFinishAt(spell, 9.6f);
        var rotation = spell.Rotation;

        foreach (var angle in angles)
        {
            rotation += angle;
            Sequences.Add(new Sequence(_shape, Module.PrimaryActor.Position, rotation, angle, activation, 0, 1));
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        var aidName = ((AID)spell.Action.ID).ToString();
        if (!aidName.StartsWith("WordOfTheWood")) return;

        if (Sequences.Count > 0)
            AdvanceSequence(0, WorldState.CurrentTime);
    }

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        int count = 0;
        foreach (var s in Sequences)
        {
            if (s.NumRemainingCasts <= 0) continue;

            yield return new AOEInstance(
                s.Shape,
                s.Origin,
                s.Rotation,
                s.NextActivation,
                count == 0 ? Colors.Danger : Colors.AOE,
                count == 0
            );

            if (++count >= 2) break;
        }
    }
}

class NechucihoStates : StateMachineBuilder
{
    public NechucihoStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Level5DeathSentence>()
            .ActivateOnEnter<SentinelRoar>()
            .ActivateOnEnter<WordOfTheWood>()
            .ActivateOnEnter<WhispersOfTheWood>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "Shinryin", GroupType = BossModuleInfo.GroupType.Hunt, GroupID = (uint)BossModuleInfo.HuntRank.A, NameID = 13362)]
public class Nechuciho(WorldState ws, Actor primary) : SimpleBossModule(ws, primary);