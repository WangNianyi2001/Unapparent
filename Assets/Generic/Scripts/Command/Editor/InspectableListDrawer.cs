using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using System.Collections;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class InspectableListDrawer<T> : NestedDrawer where T : Object {
		public SerializedProperty property;
		public ReorderableList list = null;
		public IList elements => property.TargetObject()?.GetDirectMember("elements") as IList;

		public virtual void OnAdd() {
			OnChange();
		}

		public virtual void OnChange() {
			list.GetHeight();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			DrawGUI(property, label, false);
			return list.GetHeight();
		}

		public virtual float OnElementHeight(int index) =>
			EditorGUI.GetPropertyHeight(property.IndexToElementProperty(index));

		public virtual string OnElementName(int index) => elements[index].GetType().Name;

		public virtual void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			var property = this.property.IndexToElementProperty(index);
			PropertyDrawer drawer = property.MakeDrawer();
			var name = OnElementName(index);
			drawer.OnGUI(rect, property, new GUIContent(name));
		}

		public override void DrawGUI(SerializedProperty property, GUIContent label, bool draw = true) {
			this.property = property;
			if(list == null) {
				list = new ReorderableList(elements, typeof(T));
				list.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, label);
				list.onAddCallback = _ => OnAdd();
				list.onChangedCallback = _ => OnChange();
				list.drawElementCallback = OnDrawElement;
				list.elementHeightCallback = OnElementHeight;
			}
			Rect position = EditorGUI.IndentedRect(MakeArea(list.GetHeight()));
			if(draw)
				list.DoList(position);
		}
	}
}
