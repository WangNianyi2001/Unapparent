using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class NestedDrawer : DrawerBase {
		public delegate bool PropertyFilter(PropertyAccessor accessor);
		public PropertyFilter propertyFilter = declaredProperties;

		public static PropertyFilter declaredProperties = (PropertyAccessor accessor) =>
			accessor.type.GetField(
				(accessor as PropertyAccessor.Field).name,
				BindingFlags.Instance |
				BindingFlags.DeclaredOnly |
				BindingFlags.Public |
				BindingFlags.NonPublic
			) != null;

		public static PropertyFilter isPropertyOf(Type type) =>
			(PropertyAccessor accessor) =>type.IsAssignableFrom((
				accessor.root.type.GetField(
					(accessor as PropertyAccessor.Field).name,
					BindingFlags.Instance |
					BindingFlags.Public |
					BindingFlags.NonPublic
			))?.DeclaringType);

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			position.height = 0;
			draw = false;
			DrawProperty(PropertyAccessor.FromProperty(property), label);
			draw = true;
			return position.height;
		}

		protected Dictionary<string, PropertyDrawer> drawerCache = new Dictionary<string, PropertyDrawer>();

		public void DrawProperty(PropertyAccessor accessor, GUIContent label) {
			var property = accessor?.MakeProperty();
			if(property == null)
				return;
			Type drawerType = DrawerTypeGetter.Closest(accessor.type);
			if(draw)
				EditorGUI.BeginChangeCheck();
			if(drawerType != null) {
				if(GetType().Equals(drawerType)) {
					if(accessor.value == null)
						NullGUI(accessor, label);
					else
						InstanceGUI(accessor, label);
				} else {
					var key = accessor.ToString();
					PropertyDrawer drawer =
						drawerCache.ContainsKey(key) ? drawerCache[key] :
						(drawerCache[key] = Activator.CreateInstance(drawerType) as PropertyDrawer);
					if(draw)
						drawer.OnGUI(TempArea(), property, label);
					position.height += drawer.GetPropertyHeight(property, label);
				}
			}
			//else if(accessor.isArray) {
			//	int length = (accessor.value as IList).Count;
			//	for(int i = 0; i < length; ++i)
			//		DrawProperty(accessor.GetElement(i), new GUIContent("Element"));
			//}
			else {
				Property(property, label);
			}
			MakeArea(EditorGUIUtility.standardVerticalSpacing);
			if(draw && EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();
		}

		public virtual void InstanceGUI(PropertyAccessor accessor, GUIContent label) {
			return;
			if(label != null && !label.Equals(GUIContent.none))
				Label(label);
			++EditorGUI.indentLevel;
			var childProperty = accessor?.MakeProperty();
			for(bool end = childProperty.Next(true); end; end = childProperty.NextVisible(false)) {
				var childAccessor = PropertyAccessor.FromProperty(childProperty);
				if(childAccessor == null)
					continue;
				if(!propertyFilter(childAccessor))
					continue;
				var childLabel = new GUIContent(childProperty.displayName);
				DrawProperty(childAccessor, childLabel);
			}
			--EditorGUI.indentLevel;
		}

		public virtual void NullGUI(PropertyAccessor accessor, GUIContent label) {
			Label(label);
			Label(new GUIContent("Object is null"));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this.position = position;
			this.position.height = 0;
			DrawProperty(PropertyAccessor.FromProperty(property), label);
		}
	}
}
