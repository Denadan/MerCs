using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    public interface ITileEventReceiver : IEventSystemHandler
    {
        void MouseTileEnter(Vector2Int coord);
        void MouseTileLeave(Vector2Int coord);
        void MouseTileClick(Vector2Int coord, PointerEventData.InputButton button);
    }

    public interface IUnitEventReceiver : IEventSystemHandler
    {
        void MouseUnitEnter(UnitInfo unit);
        void MouseUnitLeave(UnitInfo unit);
        void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button);
    }

    public interface IPilotDamaged : IEventSystemHandler
    {
        void PilotDamaged(PilotHp hp);
    }

    public interface IUnitDamaged : IEventSystemHandler
    {
        void UnitDamage(UnitHp hp);
    }

    public interface IPartDamaged : IEventSystemHandler
    {
        void PartDamaged(UnitHp hp);
    }
}