using System;
using UnityEditor;

namespace Unapparent {
	[Serializable]
	public abstract class Command : ManagedAsset, IInspectable, IDisposable {
		public class TypeMenu : SelectMenu<Type, TypeMenu.Labelizer> {
			public class Labelizer : Labelizer<Type> {
				public new static string Labelize(Type obj) => obj.Name;
			}

			public static TypeMenu
				statement = new TypeMenu {
					typeof(Reference),
					"Logue",
					typeof(Monologue),
					typeof(Logue),
					"Control",
					typeof(Conditional),
					typeof(Sequential),
					typeof(Agent),
					"Character",
					typeof(SwitchState),
					typeof(NavigateTo),
				},
				condition = new TypeMenu {
					typeof(Reference),
					typeof(BoolConstant),
				},
				listener = new TypeMenu {
					typeof(OnStart),
				};
		}

		public Command parent = null;

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

		~Command() => Dispose();

		public static Command Create(Type type, Command parent) {
			Command command = CreateInstance(type) as Command;
#if UNITY_EDITOR
			command.guid = CreateAsset(command, "Command");
#endif
			command.parent = parent;
			command.SetDirty();
			return command;
		}

		public static T Create<T>(Command parent) where T : Command => Create(typeof(T), parent) as T;

		public abstract object Execute(Carrier target);

		public abstract void Inspect(ArgList<Action> elements);
		public void Inspect(params Action[] elements) => Inspect(new ArgList<Action>(elements));

		public void ShowRefBtn() {
			if(IGUI.Button("Ref"))
				EditorGUIUtility.PingObject(this);
		}
	}
}
