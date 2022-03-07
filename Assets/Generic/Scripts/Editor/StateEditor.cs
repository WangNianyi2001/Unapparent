using System;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	[CustomEditor(typeof(State))]
	public class StateEditor : Editor {
		State state;
		InspectorList<Listener> iList;

		public void OnEnable() {
			state = serializedObject.targetObject as State;
		}

		public override void OnInspectorGUI() {
			if(Application.isPlaying) {
				IGUI.Center(() => IGUI.Italic("Editing state during play mode is not supported."));
				return;
			}

			if(iList == null) {
				iList = new InspectorList<Listener>(state.listeners, "Listeners");
				iList.actionButtons.Add(new InspectorList<Listener>.ActionButton {
					label = "Append",
					action = () => {
						Command.TypeMenu.listener.Show((Type type) => {
							state.listeners.Add(Listener.Create(type));
							EditorUtility.SetDirty(state);
						});
					}
				});
				iList.moreOptions.Add(new InspectorList<Listener>.MoreOption {
					label = "Remove",
					action = (Listener listener, int i) => {
						listener?.Dispose();
						state.listeners.RemoveAt(i);
					}
				});
			}
			iList.Inspect();
		}
	}
}
