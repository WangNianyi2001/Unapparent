using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class NestedDrawer : PropertyDrawer {
		protected Rect position;

		public Rect MakeArea(float height) {
			Rect rect = new Rect(position.xMin, position.yMax, position.width, height);
			position.height += height;
			return rect;
		}

		public Rect MakeArea() => MakeArea(EditorGUIUtility.singleLineHeight);

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => position.height;

		public Rect PropertyArea(SerializedProperty property) => MakeArea(EditorGUI.GetPropertyHeight(property));

		public delegate bool PropertyFilter(SerializedProperty property);

		public static PropertyFilter allProperties = (SerializedProperty _) => true;

		public static PropertyFilter declaredProperties = (SerializedProperty property) => {
			Type type = property.serializedObject.targetObject.GetType();
			const BindingFlags bindingFlags = BindingFlags.Instance |
				BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic;
			return type.GetField(property.name, bindingFlags) != null;
		};

		public static PropertyFilter isPropertyOf(Type type) => (SerializedProperty property) => {
			Type parentType = property.serializedObject.targetObject.GetType();
			const BindingFlags bindingFlags = BindingFlags.Instance |
				BindingFlags.Public | BindingFlags.NonPublic;
			var fi = parentType?.GetField(property.name, bindingFlags);
			return type.IsAssignableFrom(fi?.DeclaringType);
		};

		public PropertyFilter propertyFilter = declaredProperties;

		public void DrawProperty(SerializedProperty property) {
			EditorGUI.BeginChangeCheck();
			var label = new GUIContent(property.displayName);
			switch(property.propertyType) {
				case SerializedPropertyType.Generic:
				case SerializedPropertyType.ObjectReference:
					Type propType = property.TargetType();
					Type drawerType = EditorAux.ClosestDrawerType(property);
					if(drawerType == null)
						goto default;
					if(drawerType.Equals(GetType())) {
						DrawGUI(property, label);
						break;
					}
					var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
					drawer.OnGUI(position, property, label);
					break;
				default:
					EditorGUI.PropertyField(PropertyArea(property), property, label, true);
					break;
			}
			if(EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();
		}

		public void DrawProperties(Object target) {
			if(target == null)
				return;
			var child = new SerializedObject(target).GetIterator();
			for(bool end = child.Next(true); end; end = child.NextVisible(false)) {
				if(propertyFilter(child.Copy()))
					DrawProperty(child);
			}
		}

		public virtual void DrawGUI(SerializedProperty property, GUIContent label) {
			EditorGUI.LabelField(MakeArea(), label);
			++EditorGUI.indentLevel;
			DrawProperties(EditorAux.PropertyToObject(property) as Object);
			--EditorGUI.indentLevel;
		}

		public virtual void NullGUI(GUIContent label) {
			EditorGUI.LabelField(MakeArea(), label, new GUIContent("Object is null"));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this.position = position;
			this.position.height = 0;
			DrawProperty(property);
		}
	}
}
