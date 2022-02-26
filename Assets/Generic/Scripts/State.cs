using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace Unapparent {

	[ExecuteInEditMode]
	public class State : MonoBehaviour {
		[Serializable]
		public struct Listener {
			public UnityEvent uEvent;
			[SerializeField] public Command command;
		}

		[SerializeField] public List<Listener> listeners = new List<Listener>();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(State))]
	public class StateEditor : Editor, IDisposable {
		State state;

		void Add() {
			State.Listener listener;
			listener.uEvent = null;
			listener.command = Command.Create(typeof(Sequential));
			state.listeners.Add(listener);
		}

		public void Dispose() {
			if(EditorUtility.DisplayDialog("Warning",
				"You're about to remove this state component.",
				"Continue", "Cancel")) {
				foreach(State.Listener listener in state.listeners)
					listener.command?.Dispose();
				if(Application.isEditor) DestroyImmediate(state);
				else Destroy(state);
			}
		}

		public void OnEnable() {
			state = serializedObject.targetObject as State;
		}

		public override void OnInspectorGUI() {
			IGUI.Center(() => IGUI.Italic("Do not remove/reset via the default dropdown menu."));

			// Draw listeners list
			EditorGUILayout.LabelField("Listeners");
			SerializedProperty listeners = serializedObject.FindProperty("listeners");
			listeners.Next(true);
			listeners.Next(true);
			listeners.Next(true);
			IGUI.Indent(() => {
				for(int i = 0; i < state.listeners.Count; ++i, listeners.Next(false)) {
					int j = i;
					State.Listener listener = state.listeners[j];
					IGUI.Inline(() => {
						IGUI.Label("When");
						EditorGUILayout.PropertyField(listeners.FindPropertyRelative("uEvent"));
					});
					listener.command?.Inspect(() => IGUI.Label("Do"));
					if(IGUI.Button("Remove listener", IGUI.exWidth)) {
						listener.command?.Dispose();
						state.listeners.RemoveAt(j);
					}
					IGUI.HorizontalLine();
				}
			});
			
			if(IGUI.Button("Add listener", IGUI.exWidth))
				Add();
			if(IGUI.Button("Remove component", IGUI.exWidth))
				Dispose();
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}
