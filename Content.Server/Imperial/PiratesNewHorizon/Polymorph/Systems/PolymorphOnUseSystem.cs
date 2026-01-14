using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;
using Content.Server.Polymorph.Systems;
using Content.Shared.Polymorph;
using Content.Server.Imperial.PiratesNewHorizon.Polymorph.Components;
using Content.Server.Polymorph.Components;
using Robust.Shared.Prototypes;
using Content.Shared.Whitelist;

namespace Content.Server.Imperial.PiratesNewHorizon.Polymorph.Systems;

public sealed class PolymorphOnUseSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly PolymorphSystem _polymorph = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PolymorphOnUseComponent, UseInHandEvent>(OnUseInHand);
    }

    private void OnUseInHand(Entity<PolymorphOnUseComponent> entity, ref UseInHandEvent args)
    {
        if (_whitelistSystem.CheckBoth(args.User, entity.Comp.Whitelist, entity.Comp.Blacklist))
        {
            return;
        }
        if (entity.Comp.AlwaysRevert == true && TryComp<PolymorphedEntityComponent>(args.User, out var polymorphed))
        {
            _polymorph.Revert((args.User, polymorphed));
        }
        else
        {
            if (entity.Comp.Configuration == null)
            {
                return;
            }
            var config = entity.Comp.Configuration ?? new ProtoId<PolymorphPrototype>();
            if (entity.Comp.TogglePolymorph == true)
            {
                if (TryComp<PolymorphedEntityComponent>(args.User, out var polymorphed2))
                {
                    _polymorph.Revert((args.User, polymorphed2));
                }
                else
                {
                    _polymorph.PolymorphEntity(args.User, config);
                }
            }
            else
            {
                _polymorph.PolymorphEntity(args.User, config);
            }
        }
        if (entity.Comp.Uses > 0)
        {
            entity.Comp.Uses -= 1;
            if (entity.Comp.Uses == 0)
            {
                if (entity.Comp.DestroyOnEnd == true)
                    QueueDel(entity.Owner);
                else
                    RemComp<PolymorphOnUseComponent>(entity.Owner);
            }
        }
    }
}
