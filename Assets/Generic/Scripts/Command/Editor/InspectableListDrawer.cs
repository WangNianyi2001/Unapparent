using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.Collections;

namespace Unapparent {
	public class InspectableListDrawer<T> : NestedDrawer where T : Object {
		public SerializedProperty property;
		public object target = null;
		public IList elements => target.GetDirectMember("elements") as IList;
		public ReorderableList list = null;
		public List<PropertyDrawer> drawers = new List<PropertyDrawer>();

		void RefreshDrawers() {
			if(drawers == null)
				drawers = new List<PropertyDrawer>();
			drawers.Clear();
			var drawerType = typeof(T).ClosestDrawerType();
			for(int i = 0; i < elements.Count; ++i) {
				var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
				drawers.Add(drawer);
			}
		}

		public SerializedProperty IndexToElementProperty(int index) =>
			property.FindPropertyRelative($"elements.Array.data[{index}]");

		public virtual void OnAdd() { }

		public virtual float OnElementHeight(int index) {
			if(index >= drawers.Count)
				RefreshDrawers();
			var drawer = drawers[index];
			var property = IndexToElementProperty(index);
			drawer.OnGUI(position, property, GUIContent.none);
			return drawer.GetPropertyHeight(property, GUIContent.none);
		}

		public virtual void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			var property = IndexToElementProperty(index);
			if(property == null)
				return;
			object element = elements[index];
			var label = new GUIContent(element.GetType().Name);
			drawers[index].OnGUI(rect, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			=> (list == null ? 0 : list.GetHeight()) +
			EditorGUI.GetPropertyHeight(SerializedPropertyType.Generic, label);

		public override void DrawGUI(SerializedProperty property, GUIContent label) {
			this.property = property;
			if(target == null)
				target = property.TargetObject();
			if(list == null) {
				list = new ReorderableList(elements, typeof(T));
				list.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, label);
				list.onAddCallback = _ => OnAdd();
				list.onChangedCallback = _ => RefreshDrawers();
				list.drawElementCallback = OnDrawElement;
				list.elementHeightCallback = OnElementHeight;
			}
			Rect position = EditorGUI.IndentedRect(MakeArea(list.GetHeight()));
			list.DoList(position, position);
		}
	}
}
