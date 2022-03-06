using UnityEditor;
using UnityEngine;
using Unapparent;

[CustomPropertyDrawer(typeof(Command))]
public class CommandDrawer : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.PrefixLabel(position, label);
		Command target = property.objectReferenceValue as Command;
		target?.Inspect();
	}
}
