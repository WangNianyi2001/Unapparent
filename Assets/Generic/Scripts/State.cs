using UnityEngine;
using UnityEditor;

namespace Unapparent {
	[ExecuteInEditMode]
	public class State : MonoBehaviour {
		[SerializeField]
		public Command command;

#if UNITY_EDITOR
		// Restore serialized command reference to ScriptableObject at editor launch
		public void Start() {
			if(command == null)
				command = Command.Create(typeof(Sequential));
			else {
				string path = AssetDatabase.GUIDToAssetPath(command.guid);
				command = AssetDatabase.LoadAssetAtPath<Command>(path);
			}
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(State))]
	public class StateEditor : Editor {
		public override void OnInspectorGUI() {
			IGUI.Center(() => IGUI.Italic("Do not remove/reset via the default dropdown menu."));
			DrawDefaultInspector();
			if(IGUI.Button("Remove component", IGUI.exWidth)) {
				if(EditorUtility.DisplayDialog("Warning",
					"You're about to remove this state component.",
					"Continue", "Cancel")) {
					State state = serializedObject.targetObject as State;
					state.command?.Dispose();
					if(Application.isEditor) DestroyImmediate(state);
					else Destroy(state);
				}
			}
		}
	}

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
