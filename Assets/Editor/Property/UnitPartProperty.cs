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
            var w = position.width / 13;

            var style = GUI.skin.GetStyle("label");
            style.alignment = TextAnchor.LowerRight;

            var back = property.FindPropertyRelative("HasBackArmor");
            EditorGUI.PropertyField(new Rect(p.x, p.y, w * 2, 14), property.FindPropertyRelative("Place"), GUIContent.none);
            EditorGUI.LabelField(new Rect(p.x + w * 2, p.y, w, 14), "St:", style);
            EditorGUI.PropertyField(new Rect(p.x + w * 3, p.y, w * 2, 14), property.FindPropertyRelative("Structure"), GUIContent.none);

            EditorGUI.LabelField(new Rect(p.x + w * 5, p.y, w, 14), "Ar:", style);

            bool back_value = EditorGUI.Toggle(new Rect(p.x + w * 6 + w / 2 - 7, p.y, 14, 14), back.boolValue);
            back.boolValue = back_value;
            if (back_value)
            {
                EditorGUI.PropertyField(new Rect(p.x + w * 7, p.y, w * 1f, 14), property.FindPropertyRelative("Armor"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(p.x + w * 8, p.y, w * 1f, 14), property.FindPropertyRelative("BackArmor"), GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(new Rect(p.x + w * 7, p.y, w * 2f, 14), property.FindPropertyRelative("Armor"), GUIContent.none);
            }

            EditorGUI.PropertyField(new Rect(p.x + w * 9, p.y, w * 2, 14), property.FindPropertyRelative("DependOn"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(p.x + w * 11, p.y, w * 2, 14), property.FindPropertyRelative("TransferTo"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(p.x + w * 13, p.y, w, 14), property.FindPropertyRelative("Critical"), GUIContent.none);

            EditorGUI.LabelField(new Rect(p.x + w * 2, p.y + 16, w*2, 14), "Critical:", style);
            EditorGUI.PropertyField(new Rect(p.x + w * 4, p.y + 16, w, 14), property.FindPropertyRelative("Critical"), GUIContent.none);


            EditorGUI.LabelField(new Rect(p.x + w * 5, p.y + 16, w, 14), "F:", style);
            EditorGUI.PropertyField(new Rect(p.x + w * 6, p.y + 16, w, 14), property.FindPropertyRelative("Size").GetArrayElementAtIndex(0), GUIContent.none);
            EditorGUI.LabelField(new Rect(p.x + w * 7, p.y + 16, w, 14), "L:", style);
            EditorGUI.PropertyField(new Rect(p.x + w * 8, p.y + 16, w, 14), property.FindPropertyRelative("Size").GetArrayElementAtIndex(1), GUIContent.none);
            EditorGUI.LabelField(new Rect(p.x + w * 9, p.y + 16, w, 14), "R:", style);
            EditorGUI.PropertyField(new Rect(p.x + w * 10, p.y + 16, w, 14), property.FindPropertyRelative("Size").GetArrayElementAtIndex(2), GUIContent.none);
            EditorGUI.LabelField(new Rect(p.x + w * 11, p.y + 16, w, 14), "B:", style);
            EditorGUI.PropertyField(new Rect(p.x + w * 12, p.y + 16, w, 14), property.FindPropertyRelative("Size").GetArrayElementAtIndex(3), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }
    }
}
