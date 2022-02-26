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
#if UNITY_EDITOR
			if(!AssetDatabase.IsValidFolder(commandsPath)) {
				bool success = AssetDatabase.CreateFolder(sceneDir, commandsFolderName).Length != 0;
				if(!success) {
					Debug.LogWarning($"Folder {commandsPath} creation failed");
					return null;
				}
			}
#endif
			Command command = CreateInstance(type) as Command;
			command.parent = parent;
#if UNITY_EDITOR
			string path = AssetDatabase.GenerateUniqueAssetPath($"{commandsPath}/Command.asset");
			AssetDatabase.CreateAsset(command, path);
			command.guid = AssetDatabase.AssetPathToGUID(path);
			// Debug.Log($"Creating {command.guid}");
			command.SetDirty();
			AssetDatabase.RenameAsset(path, command.guid);
#endif
			return command;
		}

		public abstract object Execute();

		public abstract void Inspect(ArgList<Action> elements);
		public void Inspect(params Action[] elements) => Inspect(new ArgList<Action>(elements));
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Command))]
	public class CommandDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.PrefixLabel(position, label);
			Command target = property.objectReferenceValue as Command;
			target?.Inspect();
		}
	}
#endif
}
