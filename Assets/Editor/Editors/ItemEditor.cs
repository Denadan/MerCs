using UnityEditor;

namespace Mercs.Editor
{
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (targets.Length == 1)
            {
                EditorGUILayout.TextArea(targets[0].ToString());
            }
        }
    }
}