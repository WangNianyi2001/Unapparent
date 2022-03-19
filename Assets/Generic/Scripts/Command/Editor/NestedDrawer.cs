using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class NestedDrawer : PropertyDrawer {
		protected Rect position;

		public Rect TempArea(float height = 0) =>
			new Rect(position.xMin, position.yMax, position.width, height);

		public Rect MakeArea(float height) {
			Rect rect = TempArea(height);
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

		public void DrawProperty(SerializedProperty property, GUIContent label) {
			if(property == null)
				return;
			EditorGUI.BeginChangeCheck();
			Type drawerType = property.ClosestDrawerType();
			if(drawerType == null) {
				EditorGUI.PropertyField(PropertyArea(property), property, label, true);
			} else if(drawerType.Equals(GetType())) {
				if(property.TargetObject() == null)
					NullGUI(property, label);
				else
					DrawGUI(property, label);
			} else {
				var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
				drawer.OnGUI(TempArea(), property, label);
				position.height += drawer.GetPropertyHeight(property, label);
			}
			if(EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();
		}

		public virtual void DrawGUI(SerializedProperty property, GUIContent label) {
			if(label != null && !label.Equals(GUIContent.none))
				EditorGUI.LabelField(MakeArea(), label);
			Object target = property.TargetObject() as Object;
			if(target == null)
				return;
			++EditorGUI.indentLevel;
			var child = new SerializedObject(target).GetIterator();
			for(bool end = child.Next(true); end; end = child.NextVisible(false)) {
				if(!propertyFilter(child))
					continue;
				var childLabel = new GUIContent(child.displayName);
				DrawProperty(child.Copy(), childLabel);
			}
			--EditorGUI.indentLevel;
		}

		public virtual void NullGUI(SerializedProperty property, GUIContent label) {
			EditorGUI.LabelField(MakeArea(), label, new GUIContent("Object is null"));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this.position = position;
			this.position.height = 0;
			DrawProperty(property, label);
		}
	}
}
