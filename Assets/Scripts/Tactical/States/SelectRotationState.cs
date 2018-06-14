using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    /// <summary>
    /// Базовый класс для выбора вращения
    /// </summary>
    public abstract class SelectRotationState : TacticalStateHandler
    {
        /// <summary>
        /// точка в мире откуда вращать
        /// </summary>
        private Vector3 origin;

        private Vector2Int origin_cell;
        private Dir cur_facing;

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
            if(Cancelable && button == PointerEventData.InputButton.Right)
                Cancel();
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if (Cancelable && button == PointerEventData.InputButton.Right)
                Cancel();
        }

        public override void Update()
        {
            //получаем координаты мыши 
            var dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dest.z = origin.z;

            //считаем новый поворот
            var new_facing = DirHelper.GetRotation(origin, dest);

            //если поворот доступен и изменился
            if (Allowed(new_facing) && cur_facing != new_facing)
            {
                //вращаем юнит
                SetFacing(new_facing);
                //рисуем подсветку
                ShowFacing();
            }

            // левая кнопка - завершить
            if (Input.GetMouseButtonUp(0))
            {
                Done(cur_facing);
            }

            if (Cancelable && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
                Cancel();
        }

        /// <summary>
        /// рисуем оверлей
        /// </summary>
        protected virtual void ShowFacing()
        {
            TacticalController.Instance.Overlay.HideAll();

            //направления оверлея
            Dir Main = cur_facing;
            Dir Left = Main.TurnLeft();
            Dir Right = Main.TurnRight();
            //начальная точка
            var coord_main = origin_cell;
            //направления по которым будем двигатся при рисовании
            Dir left_back = Right.Inverse();
            Dir right_back = Left.Inverse();

            // рисуем тайл под юнитом
            TacticalController.Instance.Overlay.ShowTile(coord_main, Color.green, MapOverlay.Sector2(Main));

            //рисуем на 25 тайлов
            for (int n = 1; n <= 25; n++)
            {
                // сдвигаемся вдоль главного направления вперед
                coord_main += Main.GetDirShift(coord_main);
                //левая метка
                var c_left = coord_main;
                //правая метка
                var c_right = coord_main;

                //рисуем главную линию
                (var tex, var color) = TileInfo(coord_main, true);
                TacticalController.Instance.Overlay.ShowTile(coord_main, color, tex);

                //уходим в стороны от неё
                for (int i = 0; i < n; i++)
                {
                    c_left += left_back.GetDirShift(c_left);
                    c_right += right_back.GetDirShift(c_right);

                    //рисуем влево
                    if (TacticalController.Instance.Map.OnMap(c_left))
                    {
                        (tex, color) = TileInfo(c_left);
                        TacticalController.Instance.Overlay.ShowTile(c_left, color, tex);
                    }
                    //рисуем вправо
                    if (TacticalController.Instance.Map.OnMap(c_right))
                    {
                        (tex, color) = TileInfo(c_right);
                        TacticalController.Instance.Overlay.ShowTile(c_right, color, tex);
                    }
                }
            }
        }

        /// <summary>
        /// получить информацию что рисовать на проверяемом тайле
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="main"></param>
        /// <returns></returns>
        protected virtual (MapOverlay.Texture tex, Color color) TileInfo(Vector2Int coord, bool main = false)
        {
            Color c = Color.white;

            c.a = Mathf.Clamp(1 - TacticalController.Instance.Grid.MapDistance(origin_cell, coord) / 10, 0, 1);

            //c.a = TacticalController.Instance.Map.Distance(original.position, coord) < 10 ? 0.5f : 0;

            if (main)
                return (MapOverlay.Texture.White50, c);
            else
                return (MapOverlay.Texture.White25, c);
        }


        public override void OnLoad()
        {
            (origin_cell, cur_facing) = GetOrigin();
            origin = TacticalController.Instance.Grid.CellToWorld(origin_cell);

            ShowFacing();
        }

        public override void OnUnload()
        {
            TacticalController.Instance.Overlay.HideAll();
        }

        protected virtual void SetFacing(Dir new_facing)
        {
            cur_facing = new_facing;
        }

        /// <summary>
        /// можно ли повернуться в указанном направлении
        /// </summary>
        /// <param name="newFacing"></param>
        /// <returns></returns>
        protected abstract bool Allowed(Dir newFacing);
        /// <summary>
        /// получить точку откуда считать поворот
        /// </summary>
        /// <returns></returns>
        protected abstract (Vector2Int, Dir) GetOrigin();
        /// <summary>
        /// завершить поворот
        /// </summary>
        protected abstract void Done(Dir new_dir);
        /// <summary>
        /// отменить поворот
        /// </summary>
        protected abstract void Cancel();

        protected abstract bool Cancelable { get; }
    }
}