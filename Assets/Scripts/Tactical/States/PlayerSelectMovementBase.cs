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

        protected List<(UnitInfo unit, LineRenderer line)> lines = new List<(UnitInfo unit, LineRenderer line)>();

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

            int n = 0;

            foreach (var unit in TacticalController.Instance.Units
                .Where(u => u.Faction != GameController.Instance.PlayerFaction && u.CurrentVision > Visibility.Level.None))
            {
                lines.Add((unit, TacticalUIController.Instance.GetLine(n)));
                lines[n].line.SetPosition(1, unit.transform.position);
                n += 1;
            }
        }

        public override void OnUnload()
        {
            TacticalController.Instance.Overlay.HideAll();
            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
            foreach (var item in lines)
                item.line.gameObject.SetActive(false);
            lines.Clear();
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

        protected virtual Vector2 LineScale(List<path_node> path)
        {
            return new Vector2((path.Count - 1) * 4, 1);
        }

        public override void TileEnter(Vector2Int coord)
        {
            // получаем путь до точки
            var path = GetPath(coord);
            if (path.list != null && path.list.Count >= 2)
            {
                // рисуем линию
                var line = TacticalUIController.Instance.MoveLine;
                line.gameObject.SetActive(true);
                line.startColor = Color.red;
                line.endColor = Color.green;
                var points = (from v2 in path.list
                              select TacticalController.Instance.Grid.CellToWorld(v2.coord)).ToArray();
                line.positionCount = points.Length;
                line.SetPositions(points);
                line.material.mainTextureScale = LineScale(path.list);

                //рисуем целеуказание
                var pos = TacticalController.Instance.Grid.CellToWorld(coord);
                var weapon = TacticalController.Instance.SelectedUnit.Weapons;

                //если нет кешированой информации о целях - получаем и сохраняем
                if (path.target.target_data == null)
                    path.target.target_data =
                        TacticalController.Instance.Vision.CalcFrom(TacticalController.Instance.SelectedUnit, coord)
                        .ToDictionary(i => i.target);

                //стартовая позиция линии
                var start_pos = TacticalController.Instance.Grid.CellToWorld(coord);

                foreach (var l in lines)
                {
                    //если есть информация о юните 
                    if (path.target.target_data.TryGetValue(l.unit, out var data))
                    {
                        //уровень видимости в этой точке
                        var level = TacticalController.Instance.Vision.GetLevelFor(l.unit, TacticalController.Instance.SelectedUnit, data.level);

                        //если видно
                        if (level > Visibility.Level.Sensor)
                        {
                            //расстояние до цели
                            var dist = TacticalController.Instance.Grid.MapDistance(coord, l.unit.Position.position);
                            //включаем линию
                            l.line.gameObject.SetActive(true);
                            l.line.SetPosition(0, start_pos);
                            
                            //в зависимости от прямой видимости
                            switch (data.direct)
                            {
                                //нет прямой видиости - штриховая линия
                                case Visibility.Line.Indirect:
                                    l.line.material = TacticalUIController.Instance.StrokeLineMaterial;
                                    //плотность штриховки
                                    l.line.material.mainTextureScale = new Vector2(dist * 6f, 1f);
                                    if (dist > TacticalController.Instance.SelectedUnit.Weapons.MaxIndirectRange)
                                        l.line.startColor = l.line.endColor = Color.white;
                                    else
                                        l.line.startColor = l.line.endColor = Color.red;
                                    break;
                                //частичная видимость - цельная желтая
                                case Visibility.Line.Partial:
                                    l.line.material = TacticalUIController.Instance.SolidLineMaterial;
                                    if (dist > TacticalController.Instance.SelectedUnit.Weapons.MaxFalloffRange ||
                                        dist < TacticalController.Instance.SelectedUnit.Weapons.MinRange)
                                        l.line.startColor = l.line.endColor = Color.white;
                                    else
                                        l.line.startColor = l.line.endColor = Color.yellow;
                                    break;
                                //прямая видисость - цельная красная
                                case Visibility.Line.Dirrect:
                                    l.line.material = TacticalUIController.Instance.SolidLineMaterial;
                                    if (dist > TacticalController.Instance.SelectedUnit.Weapons.MaxFalloffRange ||
                                        dist < TacticalController.Instance.SelectedUnit.Weapons.MinRange)
                                        l.line.startColor = l.line.endColor = Color.white;
                                    else
                                        l.line.startColor = l.line.endColor = Color.red;
                                    break;
                            }

                        }
                        //не видно - скрываем
                        else
                            l.line.gameObject.SetActive(false);
                    }
                    //не видели раньше - скрываем
                    else
                        l.line.gameObject.SetActive(false);
                }
            }
        }

        public override void TileLeave(Vector2Int coord)
        {
            // скрываем линию
            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
            foreach (var item in lines)
                item.line.gameObject.SetActive(false);
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
                     (data.target = CanMove(coord)) != null)
            {
                data.dir = data.target.fast_path.facing;
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
        protected abstract (path_target target, List<path_node> list) GetPath(Vector2Int coord);
        /// <summary>
        /// проверяет можно ли двигаться в указаную точку и возвращает конечную точку пути
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        protected abstract path_target CanMove(Vector2Int coord);
    }
}