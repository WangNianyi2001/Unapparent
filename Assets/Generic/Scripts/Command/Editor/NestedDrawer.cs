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

		public Rect Indented(Rect area) => EditorGUI.IndentedRect(area);

		public Rect MakeArea(float height) {
			Rect rect = TempArea(height);
			position.yMax = rect.yMax;
			return rect;
		}

		public Rect MakeArea() => MakeArea(EditorGUIUtility.singleLineHeight);

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			position.height = 0;
			DrawProperty(property, label, false);
			return position.height;
		}

		public void Property(SerializedProperty property, GUIContent label, bool draw = true) {
			var area = MakeArea(EditorGUI.GetPropertyHeight(property, label, true));
			if(draw)
				EditorGUI.PropertyField(area, property, label, true);
		}

		public void Label(GUIContent label, bool draw = true) {
			var area = MakeArea();
			if(draw)
				EditorGUI.LabelField(area, label);
		}

		public bool Button(GUIContent label, bool draw = true) {
			var area = Indented(MakeArea());
			return draw && GUI.Button(area, label);
		}

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

		public static PropertyFilter propertyFilter = declaredProperties;

		public void DrawProperty(SerializedProperty property, GUIContent label, bool draw = true) {
			if(property == null)
				return;
			Type drawerType = property.ClosestDrawerType();
			EditorGUI.BeginChangeCheck();
			if(GetType().Equals(drawerType)) {
				if(property.TargetObject() == null)
					NullGUI(property, label, draw);
				else
					DrawGUI(property, label, draw);
			} else if(drawerType == null) {
				Property(property, label, draw);
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

		public virtual void DrawGUI(SerializedProperty property, GUIContent label, bool draw = true) {
			if(label != null && !label.Equals(GUIContent.none))
				Label(label);
			Object target = property.TargetObject() as Object;
			if(target == null)
				return;
			++EditorGUI.indentLevel;
			var filter = GetType().GetStaticField("propertyFilter") as PropertyFilter;
			var child = new SerializedObject(target).GetIterator();
			for(bool end = child.Next(true); end; end = child.NextVisible(false)) {
				if(filter == null || !filter(child))
					continue;
				var childLabel = new GUIContent(child.displayName);
				DrawProperty(child.Copy(), childLabel, draw);
			}
			--EditorGUI.indentLevel;
		}

		public virtual void NullGUI(SerializedProperty property, GUIContent label, bool draw = true) {
			Label(label, draw);
			Label(new GUIContent("Object is null"), draw);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			this.position = position;
			this.position.height = 0;
			DrawProperty(property, label);
		}
	}
}
