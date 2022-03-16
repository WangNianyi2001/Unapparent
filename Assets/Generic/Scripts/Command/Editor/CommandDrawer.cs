using System;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	[CustomPropertyDrawer(typeof(Command))]
	public class CommandDrawer : NestedDrawer {
		public CommandDrawer() {
			propertyFilter = isPropertyOf(typeof(Command));
		}

		public override void NullGUI(SerializedProperty property, GUIContent label) {
			EditorGUI.LabelField(MakeArea(), label, new GUIContent("Command is null"));
		}

		public override void DrawGUI(SerializedProperty property, GUIContent label) {
			EditorGUI.LabelField(MakeArea(), property.TargetType().Name);
		}
	}
}
