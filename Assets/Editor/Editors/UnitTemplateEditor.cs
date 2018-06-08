using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Mercs.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnitTemplate))]
    public class UnitTemplateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (targets.Select(t => t as UnitTemplate).Any(t => t.NeedUpdate) && GUILayout.Button("UPDATE"))
                foreach (UnitTemplate t in targets)
                {
                    t.Update();
                    EditorUtility.SetDirty(t);
                }

            base.OnInspectorGUI();
        }
    }
}
