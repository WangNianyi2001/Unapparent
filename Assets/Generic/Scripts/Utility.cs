using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;

namespace Unapparent {
	public static partial class Utility {
		public static object GetStaticField(this Type type, string name) {
			const BindingFlags bindingFlags = BindingFlags.Static |
				BindingFlags.Public | BindingFlags.NonPublic;
			return type.GetField(name, bindingFlags)?.GetValue(type);
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
					type.GetArrayElementType(step.name) :
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
	}
}
