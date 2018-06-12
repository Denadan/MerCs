using Mercs.Items;
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

    [CanEditMultipleObjects]
    [CustomEditor(typeof(AmmoPod))]
    public class AmmoPodEditor : ItemEditor
    {
    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : ItemEditor
    {
    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Ammo))]
    public class AmmoEditor : ItemEditor
    {
    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Reactor))]
    public class ReactorEditor : ItemEditor
    {
    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HeatSink))]
    public class HeatSinkEditor : ItemEditor
    {
    }
    [CanEditMultipleObjects]
    [CustomEditor(typeof(JumpJet))]
    public class JumpJetEditor : ItemEditor
    {
    }
}