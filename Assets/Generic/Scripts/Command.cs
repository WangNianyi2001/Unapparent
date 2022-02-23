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

	public abstract class Command : ScriptableObject {
		public static string sceneDir {
			get {
				string fullPath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
				return fullPath.Substring(0, fullPath.LastIndexOf('/'));
			}
		}
		public const string commandsFolderName = "Commands";
		public static string commandsPath => $"{sceneDir}/{commandsFolderName}";

		public class TypeMenu : IGUI.SelectMenu<Type, TypeMenu.Labelizer> {
			public class Labelizer : IGUI.Labelizer<Type> {
				public new static string Labelize(Type obj) => obj.Name;
			}

			public static TypeMenu
				statement = new TypeMenu {
					typeof(Conditional),
					typeof(Sequential),
					typeof(SwitchState),
				},
				condition = new TypeMenu {
					typeof(BoolConstant),
				};
		}

		string guid;

		public virtual void Dispose() {
			string path = AssetDatabase.GUIDToAssetPath(guid);
			AssetDatabase.DeleteAsset(path);
		}

		public static Command Create(Type type) {
			if(!AssetDatabase.IsValidFolder(commandsPath)) {
				bool success = AssetDatabase.CreateFolder(sceneDir, commandsFolderName).Length != 0;
				if(!success) {
					Debug.LogWarning($"Folder {commandsPath} creation failed");
					return null;
				}
			}
			Command command = CreateInstance(type) as Command;
			string path = command.guid = $"{commandsPath}/{command.GetHashCode()}.asset";
			AssetDatabase.CreateAsset(command, path);
			command.guid = AssetDatabase.AssetPathToGUID(path);
			AssetDatabase.RenameAsset(path, command.guid);
			return command;
		}

		public static void Dispose(ref Command command) {
			if(command == null)
				return;
			command.Dispose();
			command = null;
		}
		public static void Dispose(Command command) {
			if(command == null)
				return;
			command.Dispose();
		}

		public abstract object Execute();

		public abstract void Inspect(ArgList<Action> elements);
		public void Inspect(params Action[] elements) =>
			Inspect(new ArgList<Action>(elements));
	}
}
