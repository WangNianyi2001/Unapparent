using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Unapparent {
	public class PropertyAccessor {
		public class Directory {
			public string name;
			public FieldInfo fi;
			public Type type => elementInfo == null
				? fi.FieldType
				: fi.FieldType.GenericTypeArguments[0];

			public class ElementInfo {
				public int index;
			}
			ElementInfo elementInfo = null;

			public Directory(string name, Type parent) {
				Match match = new Regex(@"^([^[]+?)(?:\[([^]]+)\])?$").Match(name);
				GroupCollection groups = match.Groups;
				this.name = groups[1].Value;
				const BindingFlags bindFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
				fi = parent.GetField(this.name, bindFlags);
				if(fi == null)
					throw new NullReferenceException("Field not found");
				string index = groups[2].Value;
				if(!string.IsNullOrWhiteSpace(index)) {
					elementInfo = new ElementInfo();
					elementInfo.index = Convert.ToInt32(index);
				}
			}

			public object GetValue(object parent) {
				object field = fi.GetValue(parent);
				if(elementInfo == null)
					return field;
				else
					return (field as IList)[elementInfo.index];
			}

			public void SetValue(object parent, object value) {
				if(elementInfo == null)
					fi.SetValue(parent, value);
				else
					(fi.GetValue(parent) as IList)[elementInfo.index] = value;
			}
		}

		public static object TracePath(object root, IEnumerable<Directory> path) {
			foreach(var dir in path)
				root = dir.GetValue(root);
			return root;
		}

		public readonly object root;
		public readonly List<Directory> path;
		public object closestParent => TracePath(root, path.GetRange(0, path.Count - 1));
		public Directory lastDir => path.Last();
		public Type type => lastDir.type;

		public PropertyAccessor(object root, string[] directories) {
			this.root = root;
			path = new List<Directory>();
			directories.Aggregate(root.GetType(), (Type lastType, string dirName) => {
				Directory dir = new Directory(dirName, lastType);
				path.Add(dir);
				return dir.type;
			});
		}

		public PropertyAccessor(object root, string path) : this(root, path.Split('.')) { }

		public PropertyAccessor(SerializedProperty property) : this(
			property.serializedObject.targetObject,
			property.propertyPath.Replace(".Array.data[", "[")) { }

		public object Value {
			get => lastDir.GetValue(closestParent);
			set => lastDir.SetValue(closestParent, value);
		}
	}

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
