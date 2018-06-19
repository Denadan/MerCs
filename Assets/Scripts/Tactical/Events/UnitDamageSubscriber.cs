using UnityEngine;

namespace Mercs.Tactical.Events
{
    [AddComponentMenu("Merc/Subscribers/Unit Damaged")]
    public class UnitDamageSubscriber : EventSubscriber<IUnitDamaged>
    { }
}