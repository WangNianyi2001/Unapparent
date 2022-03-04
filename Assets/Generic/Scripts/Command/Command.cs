using System;
using System.Collections.Generic;
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

	[Serializable]
	public abstract class Command : ScriptableObject, IDisposable {
		public const string commandsFolderName = "Commands";

		public class TypeMenu : IGUI.SelectMenu<Type, TypeMenu.Labelizer> {
			public class Labelizer : IGUI.Labelizer<Type> {
				public new static string Labelize(Type obj) => obj.Name;
			}

			public static TypeMenu
				statement = new TypeMenu {
					typeof(Reference),
					typeof(Conditional),
					typeof(Sequential),
					typeof(SwitchState),
				},
				condition = new TypeMenu {
					typeof(Reference),
					typeof(BoolConstant),
				};
		}

		public Command parent = null;
		public string guid;

		public new void SetDirty() {
#if UNITY_EDITOR
			// Debug.Log($"Setting {guid} dirty");
			EditorUtility.SetDirty(this);
			parent?.SetDirty();
#endif
		}

		public virtual void Dispose() {
#if UNITY_EDITOR
			// Debug.Log($"Deleting {guid}");
			string path = AssetDatabase.GUIDToAssetPath(guid);
			AssetDatabase.DeleteAsset(path);
#endif
		}

		public static Command Create(Type type, Command parent = null) {
			Command command = CreateInstance(type) as Command;
#if UNITY_EDITOR
			command.guid = ManagedAsset.CreateAsset(command, commandsFolderName);
#endif
			command.parent = parent;
			command.SetDirty();
			return command;
		}

		public abstract object Execute();

		public abstract void Inspect(ArgList<Action> elements);
		public void Inspect(params Action[] elements) => Inspect(new ArgList<Action>(elements));

		public void ShowRefBtn() {
			if(IGUI.Button("Ref"))
				EditorGUIUtility.PingObject(this);
		}
	}
}
