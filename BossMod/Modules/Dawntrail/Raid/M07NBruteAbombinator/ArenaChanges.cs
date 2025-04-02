namespace BossMod.Dawntrail.Raid.M07NBruteAbombinator;

class ArenaChanges(BossModule module) : BossComponent(module)
{
    public override void OnEventEnvControl(byte index, uint state)
    {
        if (state == 0x00020001u)
        {
            if (index == 0x00)
            {
                Arena.Bounds = M07NBruteAbombinator.RectArena;
                Arena.Center = M07NBruteAbombinator.FinalCenter;
            }
            else if (index == 0x01)
            {
                Arena.Bounds = M07NBruteAbombinator.DefaultArena;
            }
        }
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if (spell.Action.ID == (uint)AID.NeoBombarianSpecial)
        {
            Arena.Bounds = M07NBruteAbombinator.KnockbackArena;
            Arena.Center = M07NBruteAbombinator.KnockbackArena.Center;
        }
    }
}
