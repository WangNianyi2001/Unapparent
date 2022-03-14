using UnityEditor;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;

namespace Unapparent {
	public static class EditorAux {
		public static object PropertyToObject(SerializedProperty property) {
			var parent = property?.serializedObject.targetObject;
			var fi = parent?.GetType().GetField(property.propertyPath);
			return fi?.GetValue(parent);
		}

		public static object GetStaticMember(this Type type, string name) {
			const BindingFlags bindingFlags = BindingFlags.Static |
				BindingFlags.Public | BindingFlags.NonPublic;
			var fi = type.GetMember(name, bindingFlags);
			if(fi.Length == 0)
				return null;
			return fi[0];
		}

		public static FieldInfo GetDirectMemberField(this Type target, string name) {
			const BindingFlags bindFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
			if(target == null)
				return null;
			for(var type = target; type != null; type = type.BaseType) {
				var field = type.GetField(name, bindFlags);
				if(field != null)
					return field;
			}
			return null;
		}

		public static Type GetDirectMemberType(this Type target, string name)
			=> target.GetDirectMemberField(name)?.FieldType;

		public static object GetDirectMember(this object target, string name)
			=> target.GetType().GetDirectMemberField(name)?.GetValue(target);

		public static Type GetArrayElementType(this Type target, string name) {
			var fi = target.GetDirectMemberField(name);
			var type = fi?.FieldType;
			if(!typeof(IEnumerable).IsAssignableFrom(type))
				return null;
			if(!type.IsGenericType)
				return typeof(object);
			return type.GenericTypeArguments[0];
		}

		public static object GetArrayElement(this object target, string name, int index) {
			IList array = target.GetDirectMember(name) as IList;
			if(array == null)
				return null;
			return array[index];
		}

		public struct MemberStep {
			public bool array;
			public string name;
			public int index;

			public MemberStep(string step) {
				Match match = new Regex(@"([^[]+)\[([^]]+)\]").Match(step);
				if(match.Success) {
					array = true;
					GroupCollection groups = match.Groups;
					name = groups[1].Value;
					index = Convert.ToInt32(groups[2].Value);
				} else {
					array = false;
					name = step;
					index = 0;
				}
			}
		}

		public static Type GetMemberType(this Type target, string path) {
			Type type = target;
			foreach(var step in path.Split('.').Select(step => new MemberStep(step))) {
				if(type == null)
					return null;
				type = step.array ?
					type.GetArrayElementType(step.name):
					type.GetDirectMemberType(step.name);
			}
			return type;
		}

		public static object GetMember(this object target, string path) {
			object result = target;
			foreach(var step in path.Split('.').Select(step => new MemberStep(step))) {
				if(result == null)
					return null;
				result = step.array ?
					result.GetArrayElement(step.name, step.index) :
					result.GetDirectMember(step.name);
			}
			return result;
		}

		public static string SystemPath(this SerializedProperty property) =>
			property.propertyPath.Replace(".Array.data[", "[");

		public static Type DrawerType(this Type type) {
			var assembly = Assembly.GetAssembly(typeof(Editor));
			var instance = assembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
			const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static |
				BindingFlags.Public | BindingFlags.NonPublic;
			var method = instance.GetType().GetMethod("GetDrawerTypeForType", bindingFlags);
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

		public static object TargetObject(this SerializedProperty property)
			=> property?.serializedObject.targetObject.GetMember(property.SystemPath());

		public static Type TargetType(this SerializedProperty property) {
			Type parentType = property.serializedObject.targetObject.GetType();
			return parentType.GetMemberType(property.SystemPath());
		}

		public static Type ClosestDrawerType(this SerializedProperty property) =>
			ClosestDrawerType(property.TargetType());
	}
}
