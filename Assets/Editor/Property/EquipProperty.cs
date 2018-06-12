﻿using Mercs.Items;
using UnityEditor;
using UnityEngine;

namespace Mercs.Editor
{
    [CustomPropertyDrawer(typeof(UnitTemplate.Equip))]
    public class EquipProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUI.BeginProperty(position, label, property);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var p = new SizeRect(position, 4);


            EditorGUI.PropertyField(p.Next(1), property.FindPropertyRelative("place"), GUIContent.none);
            var obj = property.FindPropertyRelative("Module");
            obj.objectReferenceValue = EditorGUI.ObjectField(p.Next(3), GUIContent.none, obj.objectReferenceValue, typeof(IModuleInfo), false);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}