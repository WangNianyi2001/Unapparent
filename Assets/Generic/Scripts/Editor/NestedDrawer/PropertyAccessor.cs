using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Unapparent {
	public class PropertyAccessor {
		public struct Directory {
			public readonly string name;
			public readonly bool isElement;
			public readonly int index;
			public FieldInfo fi;

			public Type fieldType => fi.FieldType;
			public bool isArray => typeof(IList).IsAssignableFrom(fieldType);
			public Type elementType => fieldType.IsGenericType ? fieldType.GenericTypeArguments[0] : typeof(object);
			public Type iterType => isElement ? elementType : fieldType;
			public Type contentType => isArray ? elementType : fieldType;

			public Directory(Type parent, string dirStr) {
				Match match = new Regex(@"^([^[]+?)(?:\[([^]]+)\])?$").Match(dirStr);
				GroupCollection groups = match.Groups;
				name = groups[1].Value;
				const BindingFlags bindFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
				fi = parent.GetField(name, bindFlags);
				if(fi == null)
					throw new NullReferenceException("Field not found");
				string indexStr = groups[2].Value;
				if(!string.IsNullOrWhiteSpace(indexStr)) {
					isElement = true;
					index = Convert.ToInt32(indexStr);
				} else {
					isElement = false;
					index = -1;
				}
			}

			public object GetValue(object parent) {
				object field = fi.GetValue(parent);
				return isElement ? (field as IList)[index] : field;
			}

			public void SetValue(object parent, object value) {
				if(isElement)
					(fi.GetValue(parent) as IList)[index] = value;
				else
					fi.SetValue(parent, value);
			}

			public override string ToString() => isElement ? $"{name}[{index}]" : name;
		}

		public readonly object root;
		public readonly List<Directory> path;

		public object value {
			get {
				object obj = root;
				for(int i = 0; i < path.Count; ++i) {
					var dir = path[i];
					if(obj == null)
						return null;
					obj = dir.GetValue(obj);
				}
				return obj;
			}
			set {
				object obj = root;
				for(int i = 0; i < path.Count - 1; ++i) {
					var dir = path[i];
					if(obj == null)
						throw new NullReferenceException("Value to set has a null in path");
					obj = dir.GetValue(obj);
				}
				path.Last().SetValue(obj, value);
			}
		}

		public Directory final => path.Last();
		public string name => final.name;

		public Type targetType => final.iterType;
		class DrawerTypeGetter {
			public Assembly assembly;
			public object instance;
			public MethodInfo method;
			public DrawerTypeGetter() {
				assembly = Assembly.GetAssembly(typeof(Editor));
				instance = assembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
				method = instance.GetType().GetMethod("GetDrawerTypeForType",
					BindingFlags.Instance | BindingFlags.Static |
					BindingFlags.Public | BindingFlags.NonPublic);
			}
			public Type get(Type type) =>
				(Type)method?.Invoke(instance, new object[] { type });
		}
		static DrawerTypeGetter drawerTypeGetter = new DrawerTypeGetter();
		public Type drawerType => drawerTypeGetter.get(targetType);
		public Type closestDrawerType {
			get {
				for(Type type = targetType; type != null; type = type.BaseType) {
					Type result = drawerTypeGetter.get(type);
					if(result != null)
						return result;
				}
				return null;
			}
		}

		public PropertyAccessor(object root, string[] dirStrs) {
			this.root = root;
			path = new List<Directory>();
			Type last = root.GetType();
			foreach(var dirSir in dirStrs) {
				Directory dir = new Directory(last, dirSir);
				path.Add(dir);
				last = dir.iterType;
			}
		}
		public PropertyAccessor(object root, string path) : this(root, path.Split('.')) { }

		public override string ToString() => string.Join("", path.Select((Directory dir) => '.' + dir.ToString()));

		public static implicit operator PropertyAccessor(SerializedProperty property) {
			try {
				return new PropertyAccessor(
					property.serializedObject.targetObject,
					property.propertyPath.Replace(".Array.data[", "["));
			} catch(Exception) {
				return null;
			}
		}

		public static implicit operator SerializedProperty(PropertyAccessor accessor) {
			var so = new SerializedObject(accessor.root as Object);
			if(so == null)
				throw new InvalidCastException();
			Directory first = accessor.path[0];
			SerializedProperty sp = so.FindProperty(first.name);
			if(first.isElement)
				sp = sp.GetArrayElementAtIndex(first.index);
			for(int i = 1; i < accessor.path.Count; ++i) {
				if(sp == null)
					return null;
				Directory dir = accessor.path[i];
				sp = sp.FindPropertyRelative(dir.name);
				if(dir.isElement)
					sp = sp.GetArrayElementAtIndex(dir.index);
			}
			return sp;
		}
	}
}
