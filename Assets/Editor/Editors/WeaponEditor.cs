using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mercs.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //if (targets.Select(t => t as UnitTemplate).Any(t => t.NeedUpdate) && GUILayout.Button("UPDATE"))
            //    foreach (UnitTemplate t in targets)
            //    {
            //        t.Update();
            //        EditorUtility.SetDirty(t);
            //    }

            base.OnInspectorGUI();

            if (targets.Length == 1)
            {
                EditorGUILayout.TextArea(targets[0].ToString());
            }
        }
    }
}