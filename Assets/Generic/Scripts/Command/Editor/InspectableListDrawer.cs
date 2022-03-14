using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class InspectableListDrawer<T> : NestedDrawer where T : Object {
		SerializedProperty property;
		InspectableList<T> target = null;
		ReorderableList list = null;
		List<PropertyDrawer> drawers = new List<PropertyDrawer>();

		void RefreshDrawers() {
			if(drawers == null)
				drawers = new List<PropertyDrawer>();
			drawers.Clear();
			var drawerType = EditorAux.ClosestDrawerType(typeof(T));
			for(int i = 0; i < target.elements.Count; ++i) {
				var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
				drawers.Add(drawer);
			}
		}

		public SerializedProperty IndexToElementProperty(int index) =>
			property.FindPropertyRelative($"elements.Array.data[{index}]");

		public virtual float OnElementHeight(int index) {
			if(index >= drawers.Count)
				RefreshDrawers();
			var drawer = drawers[index];
			SerializedProperty property = IndexToElementProperty(index);
			if(property == null)
				return 0;
			drawer.OnGUI(position, property, GUIContent.none);
			return drawer.GetPropertyHeight(property, GUIContent.none);
		}

		public virtual void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			RefreshDrawers();
			var element = IndexToElementProperty(index);
			if(element == null)
				return;
			drawers[index].OnGUI(rect, element, new GUIContent("hello"));
		}

		public override void DrawGUI(SerializedProperty property, GUIContent label) {
			this.property = property;
			if(target == null)
				target = EditorAux.PropertyToObject(property) as InspectableList<T>;
			if(list == null) {
				list = new ReorderableList(target.elements, typeof(Command));
				list.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, label);
				list.onAddCallback = _ => target.OnAdd();
				list.drawElementCallback = OnDrawElement;
				list.elementHeightCallback = OnElementHeight;
			}
			list.DoList(MakeArea(list.GetHeight()));
		}
	}
}
