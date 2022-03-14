using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;

namespace Unapparent {
	public static class EditorAux {
		public static object PropertyToObject(SerializedProperty property) {
			var parent = property?.serializedObject.targetObject;
			var fi = parent?.GetType().GetField(property.propertyPath);
			return fi?.GetValue(parent);
		}

		public static object GetDirectMember(this object target, string name) {
			const BindingFlags bindFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
			if(target == null)
				return null;
			for(var type = target.GetType(); type != null; type = type.BaseType) {
				var field = type.GetField(name, bindFlags);
				if(field != null)
					return field.GetValue(target);
				var property = type.GetProperty(name, bindFlags);
				if(property != null)
					return property.GetValue(target, null);
			}
			return null;
		}

		public static object GetMember(this object target, string path) {
			object result = target;
			foreach(var step in path.Split('.')) {
				if(result == null)
					return null;
				Match match = new Regex(@"([^[]+)\[([^]]+)\]").Match(step);
				if(match.Success) {
					GroupCollection groups = match.Groups;
					result = result.GetArrayElement(groups[1].Value, Convert.ToInt32(groups[2].Value));
				} else
					result = result.GetDirectMember(step);
			}
			return result;
		}

		public static object GetArrayElement(this object target, string name, int index) {
			IList array = target.GetDirectMember(name) as IList;
			if(array == null)
				return null;
			return array[index];
		}

		private static BindingFlags propertyDrawerBindingFlags =
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Instance |
			BindingFlags.Static;

		public static Type DrawerType(this Type type) {
			var assembly = Assembly.GetAssembly(typeof(Editor));
			var instance = assembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
			var method = instance.GetType().GetMethod("GetDrawerTypeForType", propertyDrawerBindingFlags);
			return (Type)method?.Invoke(instance, new object[] { type });
		}

		public static Type ClosestDrawerType(this Type type) {
			for(; type != null; type = type.BaseType) {
				Type result = DrawerType(type);
				if(result != null)
					return result;
			}
			return null;
		}

		public static object TargetObject(this SerializedProperty property) {
			if(property == null)
				return null;
			var path = property.propertyPath.Replace(".Array.data[", "[");
			return property.serializedObject.targetObject.GetMember(path);
		}

		public static Type TargetType(this SerializedProperty property) => property.TargetObject()?.GetType();

		public static Type ClosestDrawerType(this SerializedProperty property) =>
			ClosestDrawerType(property.TargetType());
	}
}
