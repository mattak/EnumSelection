using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EnumSelectionTool
{
    [CustomPropertyDrawer(typeof(EnumSelection))]
    public class EnumSelectionDrawer : PropertyDrawer
    {
        const float HeightLabel = 16f;
        const float HeightPopup = 18f;
        const float HeightHelpBox = 36f;

        protected List<Type> EnumTypes;
        protected List<string> EnumNames;
        protected List<string> EnumAssemblyNames;
        protected float TotalHeight = HeightLabel + HeightPopup * 2;

        protected virtual void CheckInitialize()
        {
            if (this.EnumTypes == null)
            {
                var typeNames = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(it => it.GetTypes())
                    .Where(it => it.IsEnum)
                    .Select(it =>
                    {
                        var attributes = it.GetCustomAttributes(typeof(EnumSelectionEnable), false);
                        if (!attributes.Any()) return null;

                        return new
                        {
                            Type = it,
                            Name = it.FullName,
                            Assembly = it.Assembly.GetName().Name
                        };
                    })
                    .Where(it => it != null);

                this.EnumTypes = typeNames.Select(it => it.Type).ToList();
                this.EnumNames = typeNames.Select(it => it.Name).ToList();
                this.EnumAssemblyNames = typeNames.Select(it => it.Assembly).ToList();
            }
        }

        List<string> GetEnumValues(Type type)
        {
            var array = Enum.GetValues(type);
            var result = new List<string>();

            foreach (var val in array)
            {
                var enumValue = val as Enum;
                result.Add(enumValue.ToString());
            }

            return result;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var rectLabel = new Rect(position.x, position.y, position.width, HeightLabel);
            var rectPopup1 = new Rect(position.x, position.y + HeightLabel, position.width, HeightPopup);
            var rectPopup2 = new Rect(position.x, position.y + HeightLabel + HeightPopup, position.width, HeightPopup);
            var className = property.FindPropertyRelative("ClassName");
            var classValue = property.FindPropertyRelative("ClassValue");
            var assemblyName = property.FindPropertyRelative("AssemblyName");

            this.CheckInitialize();
            EditorGUI.LabelField(rectLabel, property.name);
            this.TotalHeight = HeightLabel;

            if (this.EnumNames.Count > 0)
            {
                EditorGUI.indentLevel++;

                var classNameIndex = this.EnumNames.IndexOf(className.stringValue);
                classNameIndex = EditorGUI.Popup(rectPopup1, "Class", classNameIndex, this.EnumNames.ToArray());
                this.TotalHeight += HeightPopup;

                if (classNameIndex >= 0)
                {
                    className.stringValue = this.EnumNames[classNameIndex];
                    assemblyName.stringValue = this.EnumAssemblyNames[classNameIndex];

                    var type = this.EnumTypes[classNameIndex];
                    var values = this.GetEnumValues(type);

                    var classValueIndex = values.IndexOf(classValue.stringValue);
                    classValueIndex = EditorGUI.Popup(
                        rectPopup2,
                        "Value",
                        classValueIndex,
                        this.GetEnumValues(type).ToArray()
                    );
                    this.TotalHeight += HeightPopup;

                    if (classValueIndex >= 0)
                    {
                        classValue.stringValue = values[classValueIndex];
                    }

                    property.serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.indentLevel--;
            }
            else
            {
                var rectHelpBox = new Rect(position.x, position.y + this.TotalHeight, position.width, HeightHelpBox);
                EditorGUI.HelpBox(
                    rectHelpBox,
                    "Please define enum with attribute of EnumSelectionEnable.",
                    MessageType.Info
                );
                this.TotalHeight += HeightHelpBox;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return this.TotalHeight;
        }
    }
}