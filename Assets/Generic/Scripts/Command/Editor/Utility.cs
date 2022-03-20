using UnityEditor;
using System;
using System.Reflection;
using Unity.VisualScripting;

#if UNITY_EDITOR
namespace Unapparent {
	public static partial class Utility {
		public static SerializedProperty IndexToElementProperty(this SerializedProperty property, int index) =>
			property.FindPropertyRelative($"elements.Array.data[{index}]");

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

		public static FieldInfo TargetFieldInfo(this SerializedProperty property) =>
			property.serializedObject.targetObject.GetType().GetField(property.name);

		public static void SetTarget(this SerializedProperty property, object value) {
			SerializedObject so = property.serializedObject;
			FieldInfo fi = property.TargetFieldInfo();
			fi.SetValue(so.targetObject, value);
			so.ApplyModifiedProperties();
		}

		public static object TargetObject(this SerializedProperty property)
			=> property?.serializedObject.targetObject.GetMember(property.SystemPath());

		public static Type TargetType(this SerializedProperty property) {
			Type parentType = property.serializedObject.targetObject.GetType();
			return parentType.GetMemberType(property.SystemPath());
		}

		public static Type ClosestDrawerType(this SerializedProperty property) =>
			ClosestDrawerType(property.TargetType());

		public static PropertyDrawer MakeDrawer(this SerializedProperty property) {
			var drawerType = property.ClosestDrawerType();
			return Activator.CreateInstance(drawerType) as PropertyDrawer;
		}
	}
}
#endif
