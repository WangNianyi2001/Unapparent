using UnityEngine;
using UnityEditor;

namespace Unapparent {
	[CustomEditor(typeof(MonoCommand))]
	public class MonoCommandEditor : Editor {
		MonoCommand mc;

		public void OnEnable() {
			mc = serializedObject.targetObject as MonoCommand;
			if(mc.command == null)
				mc.command = Command.Create(typeof(Sequential));
		}

		public void Dispose() {
			mc.command?.Dispose();
			if(Application.isEditor) DestroyImmediate(mc);
			else Destroy(mc);
		}

		public override void OnInspectorGUI() {
			IGUI.Center(() => IGUI.Italic("Do not remove/reset via the default dropdown menu."));
			if(Application.isPlaying) {
				IGUI.Center(() => IGUI.Italic("Editing state during play mode is not supported."));
				return;
			}

			mc.command?.Inspect();

			if(IGUI.Button("Remove component", IGUI.exWidth)) {
				if(EditorUtility.DisplayDialog("Warning",
					"You're about to remove this state component.",
					"Continue", "Cancel")) {
					Dispose();
					EditorUtility.SetDirty(mc);
				}
			}
		}
	}
}
