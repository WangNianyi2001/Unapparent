using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class NestedDrawer : NestedDrawerBase {
		public delegate bool PropertyFilter(SerializedProperty property);

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

		public static PropertyFilter propertyFilter = declaredProperties;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			position.height = 0;
			draw = false;
			DrawProperty(property, label);
			draw = true;
			return position.height;
		}

		public void DrawProperty(SerializedProperty property, GUIContent label) {
			if(property == null)
				return;
			Type drawerType = property.ClosestDrawerType();
			EditorGUI.BeginChangeCheck();
			if(GetType().Equals(drawerType)) {
				if(property.TargetObject() == null)
					NullGUI(property, label);
				else
					InstanceGUI(property, label);
			} else if(drawerType == null) {
				Property(property, label);
			} else {
				var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
				if(draw)
					drawer.OnGUI(TempArea(), property, label);
				position.height += drawer.GetPropertyHeight(property, label);
			}
			MakeArea(EditorGUIUtility.standardVerticalSpacing);
			if(EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();
		}

		public virtual void InstanceGUI(SerializedProperty property, GUIContent label) {
			if(label != null && !label.Equals(GUIContent.none))
				Label(label);
			Object target = property.TargetObject() as Object;
			if(target == null)
				return;
			++EditorGUI.indentLevel;
			var filter = GetType().GetStaticField("propertyFilter") as PropertyFilter;
			var child = new SerializedObject(target).GetIterator();
			for(bool end = child.Next(true); end; end = child.NextVisible(false)) {
				if(!filter(child))
					continue;
				var childLabel = new GUIContent(child.displayName);
				DrawProperty(child.Copy(), childLabel);
			}
			--EditorGUI.indentLevel;
		}

		public virtual void NullGUI(SerializedProperty property, GUIContent label) {
			Label(label);
			Label(new GUIContent("Object is null"));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this.position = position;
			this.position.height = 0;
			DrawProperty(property, label);
		}
	}
}
