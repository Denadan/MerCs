using UnityEditor;
using UnityEngine;

namespace Mercs.Editor
{
    [CustomPropertyDrawer(typeof(WeaponUpgrade))]
    public class WeaponUpgradeProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            //var pos = EditorGUI.PrefixLabel(position, label);
            EditorGUI.indentLevel = 0;

            var p = position;
            var w = position.width / 22;

            var style = GUI.skin.GetStyle("label");
            style.alignment = TextAnchor.LowerRight;

            EditorGUI.PropertyField(new Rect(p.x, p.y, w * 6, 14), property.FindPropertyRelative("Type"), GUIContent.none);
            float x = p.x + w * 6;


            EditorGUI.LabelField(new Rect(x, p.y, w, 14), "+", style);
            EditorGUI.PropertyField(new Rect(x + w * 1, p.y, w * 2, 14), property.FindPropertyRelative("AddValue"), GUIContent.none);
            EditorGUI.LabelField(new Rect(x+w*3 , p.y, w, 14), "%", style);
            EditorGUI.PropertyField(new Rect(x + w * 4, p.y, w * 2, 14), property.FindPropertyRelative("AddPercent"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(x + w * 6, p.y, w * 2, 14), property.FindPropertyRelative("Max"), GUIContent.none);


            EditorGUI.LabelField(new Rect(x + w * 8, p.y, w, 14), "-", style);
            EditorGUI.PropertyField(new Rect(x + w * 9, p.y, w * 2, 14), property.FindPropertyRelative("SubValue"), GUIContent.none);
            EditorGUI.LabelField(new Rect(x + w * 11, p.y, w, 14), "%", style);
            EditorGUI.PropertyField(new Rect(x + w * 12, p.y, w * 2, 14), property.FindPropertyRelative("SubPercent"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(x + w * 14, p.y, w * 2, 14), property.FindPropertyRelative("Min"), GUIContent.none);


            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}