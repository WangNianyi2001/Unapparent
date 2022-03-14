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
			Menu<Command> menu = property.TargetType()?.GetStaticMember("menu") as Menu<Command>;
			if(menu == null)
				return;
			Type drawerType = menu.GetType().ClosestDrawerType();
			if(drawerType == null)
				return;
			PropertyDrawer drawer = Activator.CreateInstance(drawerType) as MenuDrawer<Command>;
			float height = drawer.GetPropertyHeight(property, label);
			drawer.OnGUI(MakeArea(height), null, label);
		}

		public override void DrawGUI(SerializedProperty property, GUIContent label) {
			EditorGUI.LabelField(MakeArea(), property.TargetType().Name);
		}
	}
}
