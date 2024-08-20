using DefaultNamespace.DataStructure;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    [CustomPropertyDrawer(typeof(SerializableData<,>))]
    public class SerializableDataDrawer: PropertyDrawer
    {
        private SerializedProperty key, value;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            property.Next(true);
            key = property.Copy();
            property.Next(true);
            value = property.Copy();

            Rect contentPos = EditorGUI.PrefixLabel(position, new GUIContent());

            GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);
            EditorGUI.indentLevel = 0;
            float half = contentPos.width / 2;
            contentPos.width = half;
            EditorGUIUtility.labelWidth = 45f;
            EditorGUI.PropertyField(contentPos, key);
            contentPos.x += half;
            EditorGUI.PropertyField(contentPos, value);
        }
    }
}