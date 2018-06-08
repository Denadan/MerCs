using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static Mercs.Tactical.PathMap;

namespace Mercs.Tactical.States
{
    public abstract class PlayerSelectMovementBase : TacticalStateHandler
    {
        protected PhasePrepareState state;
        protected MovementStateData data;

        public path_target Path { get; private set; }

        public PlayerSelectMovementBase(PhasePrepareState state, MovementStateData data)
        {
            this.state = state;
            this.data = data;
        }

        public override void OnLoad()
        {
 //           TacticalController.Instance.Overlay.HideAll();
            // если карта путей готова - нарисовать её 
            if (TacticalController.Instance.Path.Ready)
                ShowOverlay();
            else
                TacticalController.Instance.StateMachine.StartCoroutine(wait_for_path(TacticalController.Instance.SelectedUnit));
        }

        public override void OnUnload()
        {
            TacticalController.Instance.Overlay.HideAll();
            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
        }

        private IEnumerator wait_for_path(UnitInfo info)
        {
            
            float start = TacticalController.Instance.Path.DEBUG_TimeStart;
            UnityEngine.Debug.Log($"Started");
            float end = 0;

            //ждем пока не будет готов путь
            while (!TacticalController.Instance.Path.Ready)
            {
                // если путь уже не нужен - выходим
                if (TacticalController.Instance.StateMachine.State != State ||
                    TacticalController.Instance.SelectedUnit != info)
                {
                    end = Time.realtimeSinceStartup;
                    UnityEngine.Debug.Log($"State changed\nStart at {start:0.000}s\nEnd at {end:0.000}s\nTotal: {end - start:0.000}s");

                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
            end = Time.realtimeSinceStartup;
            UnityEngine.Debug.Log($"Task Completed\nStart at {start:0.000}s\nEnd at {end:0.000}s\nTotal: {end - start:0.000}s");
            // отрисовываем
            ShowOverlay();
        }

        public override void UnitEnter(UnitInfo unit)
        {
            if (unit.Selectable)
                TacticalController.Instance.HighlightUnit(unit);
        }

        public override void UnitLeave(UnitInfo unit)
        {
            TacticalController.Instance.HideHighlatedUnit(unit);
        }

        public override void TileEnter(Vector2Int coord)
        {
            // получаем путь до точки
            var list = GetPath(coord);
            if (list != null && list.Count >= 2)
            {
                // рисуем линию
                var line = TacticalUIController.Instance.MoveLine;
                line.gameObject.SetActive(true);
                line.startColor = Color.red;
                line.endColor = Color.green;
                var points = (from v2 in list
                              select TacticalController.Instance.Grid.CellToWorld(v2.coord)).ToArray();
                line.positionCount = points.Length;
                line.SetPositions(points);
            }
        }

        public override void TileLeave(Vector2Int coord)
        {
            // скрываем линию
            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            // если кликнули на юните - отменить вращение
            if (unit.Selectable && unit != TacticalController.Instance.SelectedUnit && button == PointerEventData.InputButton.Left)
            {
                TacticalController.Instance[TacticalController.Instance.SelectedUnit].Background.color = Color.white;
                TacticalController.Instance.SelectedUnit = unit;
                TacticalUIController.Instance.ShowActionBarButtons(unit);
                TacticalUIController.Instance.ShowActionBarButton(ActionButton.Cancel);
                TacticalController.Instance[unit].Background.color = Color.green;
                TacticalController.Instance.Overlay.HideAll();

                TacticalController.Instance.Path.MakePathMap(unit);

                OnLoad();
            }

        }

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {

            // правая кнопка - отменить выбор
            if (button == PointerEventData.InputButton.Right &&
                state.ActiveUnits.Count(unit => unit.Faction == GameController.Instance.PlayerFaction) > 1)
            {
                CancelSelection();
            }
            // левая кнопка - вращать
            else if (button == PointerEventData.InputButton.Left &&
                     (data.target = CanMove(coord))!= null)
            {
                SwitchTo(TacticalState.SelectRotation);
            }
        }


        public override void Update()
        {
            //если нажата правая кнопка - отменить движение
            if (Input.GetMouseButtonDown(1) &&
               state.ActiveUnits.Count(unit => unit.Faction == GameController.Instance.PlayerFaction) > 1 &&
               !EventSystem.current.IsPointerOverGameObject())
                CancelSelection();
        }

        private void CancelSelection()
        {
            //отмена движения
            TacticalUIController.Instance.HideActionBar();
            TacticalController.Instance[TacticalController.Instance.SelectedUnit].Background.color = Color.white;
            TacticalController.Instance.SelectedUnit = null;
            TacticalController.Instance.Path.MakePathMap(null);

            SwitchTo(TacticalState.SelectUnit);
        }

        /// <summary>
        /// показывает оверлей с возможными путями
        /// </summary>
        protected abstract void ShowOverlay();
        /// <summary>
        /// возвращает путь до точки или null если пути нет
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        protected abstract List<path_node> GetPath(Vector2Int coord);
        /// <summary>
        /// проверяет можно ли двигаться в указаную точку и возвращает конечную точку пути
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        protected abstract path_target CanMove(Vector2Int coord);
    }
}