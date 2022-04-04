using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class NestedDrawer : DrawerBase {
		public delegate bool PropertyFilter(PropertyAccessor accessor);
		public PropertyFilter propertyFilter = declaredProperties;

		public static PropertyFilter declaredProperties = (PropertyAccessor accessor) =>
			accessor.targetType.GetField(accessor.name,
					BindingFlags.Instance |
					BindingFlags.DeclaredOnly |
					BindingFlags.Public |
					BindingFlags.NonPublic) != null;
		public static PropertyFilter isPropertyOf(Type type) => (PropertyAccessor accessor) =>
			type.IsAssignableFrom((
				accessor.root.GetType().GetField(accessor.name,
						BindingFlags.Instance |
						BindingFlags.Public |
						BindingFlags.NonPublic)
				)?.DeclaringType);

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			position.height = 0;
			draw = false;
			DrawProperty(property, label);
			draw = true;
			return position.height;
		}

		protected Dictionary<string, PropertyDrawer> drawerCache = new Dictionary<string, PropertyDrawer>();

		public void DrawProperty(PropertyAccessor accessor, GUIContent label) {
			SerializedProperty property = accessor;
			if(property == null)
				return;
			Type drawerType = accessor.closestDrawerType;
			EditorGUI.BeginChangeCheck();
			if(GetType().Equals(drawerType)) {
				if(accessor.value == null)
					NullGUI(accessor, label);
				else
					InstanceGUI(accessor, label);
			} else if(drawerType == null) {
				Property(property, label);
			} else {
				var key = accessor.ToString();
				PropertyDrawer drawer;
				if(drawerCache.ContainsKey(key))
					drawer = drawerCache[key];
				else
					drawerCache[key] = drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
				if(draw)
					drawer.OnGUI(TempArea(), property, label);
				position.height += drawer.GetPropertyHeight(property, label);
			}
			MakeArea(EditorGUIUtility.standardVerticalSpacing);
			if(EditorGUI.EndChangeCheck())
				property.serializedObject.ApplyModifiedProperties();
		}

		public virtual void InstanceGUI(PropertyAccessor accessor, GUIContent label) {
			if(label != null && !label.Equals(GUIContent.none))
				Label(label);
			var target = accessor.value as Object;
			if(target == null)
				return;
			++EditorGUI.indentLevel;
			var child = new SerializedObject(target).GetIterator();
			for(bool end = child.Next(true); end; end = child.NextVisible(false)) {
				PropertyAccessor childAccessor = child;
				if(childAccessor == null)
					continue;
				if(!propertyFilter(childAccessor))
					continue;
				var childLabel = new GUIContent(child.displayName);
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
			DrawProperty(property, label);
		}
	}
}
