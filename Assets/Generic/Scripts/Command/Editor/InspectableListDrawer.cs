﻿using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class InspectableListDrawer<T> : NestedDrawer where T : Object {
		public SerializedProperty property;
		public ReorderableList list = null;
		public IList elements => property.TargetObject()?.GetDirectMember("elements") as IList;

		public virtual void OnAdd() { }

		public virtual float OnElementHeight(int index) =>
			EditorGUI.GetPropertyHeight(property.IndexToElementProperty(index));

		public virtual void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
			var property = this.property.IndexToElementProperty(index);
			var name = property.TargetObject().GetType().Name;
			property.MakeDrawer().OnGUI(rect, property, new GUIContent(name));
		}

		public override void DrawGUI(SerializedProperty property, GUIContent label, bool draw = true) {
			this.property = property;
			if(list == null) {
				list = new ReorderableList(elements, typeof(T));
				list.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, label);
				list.onAddCallback = _ => OnAdd();
				list.drawElementCallback = OnDrawElement;
				list.elementHeightCallback = OnElementHeight;
			}
			Rect position = EditorGUI.IndentedRect(MakeArea(list.GetHeight()));
			if(draw)
				list.DoList(position, position);
		}
	}
}
