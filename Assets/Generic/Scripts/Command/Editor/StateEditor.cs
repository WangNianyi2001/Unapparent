using System;
using UnityEngine;
using UnityEditor;
using Unapparent;

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
