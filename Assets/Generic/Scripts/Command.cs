using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Unapparent {
	public interface ICommand : IInspectable, IDisposable {
		public object Execute();
	}

	public abstract class Command : ScriptableObject, ICommand {
		public static string sceneDir {
			get {
				string fullPath = EditorSceneManager.GetActiveScene().path;
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

		string path;

		public virtual void Dispose() {
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
			command.path = $"{commandsPath}/{command.GetHashCode()}.asset";
			AssetDatabase.CreateAsset(command, command.path);
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
		public abstract void Inspect(Action header, Action footer);
	}
}
