using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    public interface ITileEvent : IEventSystemHandler
    {
        void MouseTileEnter(Vector2Int coord);
        void MouseTileLeave(Vector2Int coord);
        void MouseTileClick(Vector2Int coord, PointerEventData.InputButton button);
    }

    public interface IUnitEvent : IEventSystemHandler
    {
        void MouseUnitEnter(UnitInfo unit);
        void MouseUnitLeave(UnitInfo unit);
        void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button);
    }

    public interface IPilotDamaged : IEventSystemHandler
    {
        void PilotDamaged(UnitInfo unit, PilotHp hp);
    }

    public interface IUnitDamaged : IEventSystemHandler
    {
        void UnitDamage(UnitInfo unit, UnitHp hp);
    }

    public interface IPartDamaged : IEventSystemHandler
    {
        void PartDamaged(UnitInfo unit, Parts part);
    }

    public interface IVisionChanged : IEventSystemHandler
    {
        void VisionChanged(UnitInfo unit, Visibility.Level level);
    }

}