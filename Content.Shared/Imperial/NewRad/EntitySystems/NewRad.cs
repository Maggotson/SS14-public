using System.Linq;
using Content.Shared.Imperial.NewRad.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Radiation.Components;
using Robust.Shared.Random;

namespace Content.Shared.Imperial.NewRad.EntitySystems;

public sealed partial class NewRadSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NewRadComponent, UseInHandEvent>(OnInHandActivation);
    }

    private void OnInHandActivation(EntityUid uid, NewRadComponent comp, UseInHandEvent ev)
    {
        var lookup = _lookup.GetEntitiesInRange(ev.User, 3);

        if (lookup is null)
            return;
        var rnd = new System.Random();
        var nm = rnd.Next(0, lookup.Count);
        var target = lookup.ElementAt(nm);
        var compRad = EnsureComp<RadiationSourceComponent>(target);
        compRad.Intensity = rnd.Next(4, 7);
        compRad.Slope = rnd.NextFloat(0.2f, 1f);
    }
}
