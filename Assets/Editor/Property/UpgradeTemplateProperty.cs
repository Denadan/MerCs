using Mercs.Items;
using UnityEditor;
using UnityEngine;

namespace Mercs.Editor
{

    [CustomPropertyDrawer(typeof(UpgradeTemplate))]
    public class UpgradeTemplateProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            //var pos = EditorGUI.PrefixLabel(position, label);
            EditorGUI.indentLevel = 0;

            var p = new SizeRect(position, 22);

            var style = GUI.skin.GetStyle("label");
            style.alignment = TextAnchor.LowerRight;

            EditorGUI.PropertyField(p.Next(6), property.FindPropertyRelative("Type"), GUIContent.none);

            EditorGUI.LabelField(p.Next(1), "+", style);
            EditorGUI.PropertyField(p.Next(2), property.FindPropertyRelative("AddValue"), GUIContent.none);
            EditorGUI.LabelField(p.Next(1), "%", style);
            EditorGUI.PropertyField(p.Next(2), property.FindPropertyRelative("AddPercent"), GUIContent.none);
            EditorGUI.PropertyField(p.Next(2), property.FindPropertyRelative("Max"), GUIContent.none);


            EditorGUI.LabelField(p.Next(1), "-", style);
            EditorGUI.PropertyField(p.Next(2), property.FindPropertyRelative("SubValue"), GUIContent.none);
            EditorGUI.LabelField(p.Next(1), "%", style);
            EditorGUI.PropertyField(p.Next(2), property.FindPropertyRelative("SubPercent"), GUIContent.none);
            EditorGUI.PropertyField(p.Next(2), property.FindPropertyRelative("Min"), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}