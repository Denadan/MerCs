using Mercs.Items;
using UnityEditor;
using UnityEngine;

namespace Mercs.Editor
{
    [CustomPropertyDrawer(typeof(Upgrade.UItem))]
    public class UpgradeItemProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            //var pos = EditorGUI.PrefixLabel(position, label);
            EditorGUI.indentLevel = 0;

            var p = new SizeRect(position, 4);

            var style = GUI.skin.GetStyle("label");
            style.alignment = TextAnchor.LowerRight;

            var type = property.FindPropertyRelative("type");
            var value = property.FindPropertyRelative("value");

            EditorGUI.PropertyField(p.Next(3), property.FindPropertyRelative("type"), GUIContent.none);
            var selected = (UpgradeType)type.intValue;
            switch(selected)
            {
                case UpgradeType.GuidanceSystem:
                    value.intValue = (int)(GuidanceSystem)EditorGUI.EnumPopup(p.Next(1), (GuidanceSystem)value.intValue);
                    break;
                default:
                    EditorGUI.PropertyField(p.Next(1), value, GUIContent.none);
                    break;
            }


        EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}