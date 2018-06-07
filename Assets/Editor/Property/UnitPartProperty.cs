using UnityEditor;
using UnityEngine;
using Mercs.Tactical;

namespace Mercs.Editor
{
    [CustomPropertyDrawer(typeof(UnitPart))]
    public class UnitPartProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            //var pos = EditorGUI.PrefixLabel(position, label);
            EditorGUI.indentLevel = 0;

            var p = position;
            var w = position.width / 14;

            var back = property.FindPropertyRelative("HasBackArmor");
            EditorGUI.PropertyField(new Rect(p.x, p.y, w * 2, p.height), property.FindPropertyRelative("Place"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(p.x + w * 2, p.y, w * 3, p.height), property.FindPropertyRelative("Structure"), GUIContent.none);
            bool back_value = EditorGUI.Toggle(new Rect(p.x + w * 6 - 14, p.y, 14, p.height), back.boolValue);
            back.boolValue = back_value;
            if (back_value)
            {
                EditorGUI.PropertyField(new Rect(p.x + w * 6, p.y, w * 1.5f, p.height), property.FindPropertyRelative("Armor"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(p.x + w * 7.5f, p.y, w * 1.5f, p.height), property.FindPropertyRelative("BackArmor"), GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(new Rect(p.x + w * 6, p.y, w * 3f, p.height), property.FindPropertyRelative("Armor"), GUIContent.none);
            }

            EditorGUI.PropertyField(new Rect(p.x + w * 9, p.y, w * 2, p.height), property.FindPropertyRelative("DependOn"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(p.x + w * 11, p.y, w * 2, p.height), property.FindPropertyRelative("TransferTo"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(p.x + w * 13, p.y, w, p.height), property.FindPropertyRelative("Critical"), GUIContent.none);
 
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
