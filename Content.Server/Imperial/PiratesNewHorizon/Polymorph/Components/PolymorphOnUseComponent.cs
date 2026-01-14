using Content.Shared.Whitelist;
using Content.Shared.Polymorph;
using Robust.Shared.Prototypes;
namespace Content.Server.Imperial.PiratesNewHorizon.Polymorph.Components;


[RegisterComponent]
public sealed partial class PolymorphOnUseComponent : Component
{
    [DataField]
    public ProtoId<PolymorphPrototype>? Configuration;
    //Entities with this component begin working like an anti-polymorph antidote
    [DataField]
    public bool AlwaysRevert = false;
    //Entities with this component begin working like a toggle switch for polymorph: if you are unpolymorphed, it polymorphs u, otherwise it reverts.
    [DataField]
    public bool TogglePolymorph = false;

    [DataField]
    public EntityWhitelist? Whitelist;

    [DataField]
    public EntityWhitelist? Blacklist;

    [DataField]
    public float Uses = 0;
    [DataField]
    public bool DestroyOnEnd = false;
}
