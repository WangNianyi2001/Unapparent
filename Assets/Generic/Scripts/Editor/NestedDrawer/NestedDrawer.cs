using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class NestedDrawer : DrawerBase {
		public delegate bool PropertyFilter(PropertyAccessor accessor);

		protected PropertyFilter propertyFilter = declaredProperties;

		const BindingFlags instanceBindingFlags = BindingFlags.Instance |
			BindingFlags.Public | BindingFlags.NonPublic;
		const BindingFlags declaredBindingFlags = instanceBindingFlags | BindingFlags.DeclaredOnly;

		public static PropertyFilter declaredProperties = (PropertyAccessor accessor) => {
			if(accessor == null)
				return false;
			string name = (accessor as PropertyAccessor.Field).name;
			return accessor.type.GetField(name, declaredBindingFlags) != null;
		};

		public static PropertyFilter isPropertyOf(Type type) => (PropertyAccessor accessor) => {
			if(accessor == null)
				return false;
			string name = (accessor as PropertyAccessor.Field).name;
			FieldInfo fi = accessor.root.type.GetField(name, instanceBindingFlags);
			return type.IsAssignableFrom(fi?.DeclaringType);
		};

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			position.height = 0;
			draw = false;
			DrawProperty(PropertyAccessor.FromProperty(property), label);
			draw = true;
			return position.height;
		}

		protected Dictionary<string, PropertyDrawer> drawerCache = new Dictionary<string, PropertyDrawer>();
		protected Dictionary<string, ReorderableList> listCache = new Dictionary<string, ReorderableList>();

		protected ReorderableList MakeList(PropertyAccessor accessor) {
			var list = new ReorderableList(accessor.value as IList, accessor.type) {
				onAddCallback = (ReorderableList list) => {
					(accessor.value as IList).Add(null);
				},
				drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
					var child = accessor.GetElement(index);
					var key = child.ToString();
					PropertyDrawer drawer = drawerCache.ContainsKey(key)
						? drawerCache[key]
						: new NestedDrawer();
					SerializedProperty property = child.MakeProperty();
					if(property != null)
						drawer.OnGUI(rect, property, GUIContent.none);
				},
				elementHeightCallback = (int index) => {
					var child = accessor.GetElement(index);
					var key = child.ToString();
					PropertyDrawer drawer = drawerCache.ContainsKey(key)
						? drawerCache[key]
						: new NestedDrawer();
					SerializedProperty property = child.MakeProperty();
					if(property != null)
						return drawer.GetPropertyHeight(property, GUIContent.none);
					else
						return 0;
				},
			};
			return list;
		}

		public void DrawProperty(PropertyAccessor accessor, GUIContent label) {
			SerializedProperty property = accessor?.MakeProperty();
			if(property == null)
				return;
			var key = accessor.ToString();
			Type drawerType = DrawerTypeGetter.Closest(accessor.type);
			EditorGUI.BeginChangeCheck();
			if(drawerType != null) {
				if(drawerType.Equals(GetType())) {
					if(accessor.value == null)
						NullGUI(accessor, label);
					else
						InstanceGUI(accessor, label);
				}
				else {
					PropertyDrawer drawer = drawerCache.ContainsKey(key)
						? drawerCache[key]
						: (drawerCache[key] = Activator.CreateInstance(drawerType) as PropertyDrawer);
					if(draw)
						drawer.OnGUI(TempArea(), property, label);
					position.height += drawer.GetPropertyHeight(property, label);
				}
			}
			else if(accessor.isArray) {
				ReorderableList list = listCache.ContainsKey(key)
					? listCache[key]
					: (listCache[key] = MakeList(accessor));
				float height = list.GetHeight();
				Rect area = MakeArea(height);
				if(draw)
					list.DoList(area);
			}
			else
				Property(property, label);
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
				PropertyAccessor childAccessor = PropertyAccessor.FromProperty(child);
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
			DrawProperty(PropertyAccessor.FromProperty(property), label);
		}
	}
}
