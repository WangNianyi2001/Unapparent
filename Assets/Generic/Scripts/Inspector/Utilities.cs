using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class ArgList<T> : List<T> {
		public ArgList(IEnumerable<T> collection) {
			InsertRange(0, collection);
		}
		public new T this[int i] {
			get => i >= 0 && i < Count ? base[i] : default(T);
		}
	}

	public interface IInspectable {
		public void Inspect(params Action[] elements);
	}

	public static class Utilities {
		public static Type GetPropertyDrawer(Type type) {
			if(type == null)
				return null;
			if(!typeof(UnityEngine.Object).IsAssignableFrom(type))
				return null;
			var assembly = Assembly.GetAssembly(typeof(Editor));
			var scriptAttributeUtility = assembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
			var scriptAttributeUtilityType = scriptAttributeUtility.GetType();
			var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
			var getDrawerTypeForType = scriptAttributeUtilityType.GetMethod("GetDrawerTypeForType", bindingFlags);
			Type result = (Type)getDrawerTypeForType.Invoke(scriptAttributeUtility, new object[] { type });
			if(result != null)
				return result;
			if(type.BaseType != null)
				return GetPropertyDrawer(type.BaseType);
			return null;
		}

		public static void UltimateDrawer(Rect position, SerializedProperty _, GUIContent label) {
			SerializedProperty property = _.serializedObject.GetIterator();
			property.Next(true);
			bool enterChild = true;
			while(property.NextVisible(enterChild)) {
				enterChild = false;
				string name = property.name;
				if(name == "m_Script" || name == "parent")
					continue;
				Type type = property.serializedObject.targetObject.GetType();
				Type drawerType = GetPropertyDrawer(type);
				if(drawerType != null) {
					PropertyDrawer drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;
					drawer.OnGUI(position, property, GUIContent.none);
				} else
					EditorGUILayout.PropertyField(property);
			}
		}
	}
}
