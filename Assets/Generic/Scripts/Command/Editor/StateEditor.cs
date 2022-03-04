﻿using System;
using UnityEngine;
using UnityEditor;
using Unapparent;

[CustomEditor(typeof(State))]
public class StateEditor : Editor, IDisposable {
	State state;

	public void Dispose() {
		foreach(Listener listener in state.listeners)
			listener.command?.Dispose();
		if(Application.isEditor) DestroyImmediate(state);
		else Destroy(state);
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
				Listener listener = state.listeners[j];
				listener?.Inspect();
				if(IGUI.Button("Remove listener", IGUI.exWidth)) {
					listener?.Dispose();
					state.listeners.RemoveAt(j);
				}
				IGUI.HorizontalLine();
			}
		});

		IGUI.SelectButton("Add listener", Command.TypeMenu.listener,
			(Type type) => state.listeners.Add(Command.Create(type) as Listener),
			IGUI.exWidth);
		if(IGUI.Button("Remove component", IGUI.exWidth)) {
			if(EditorUtility.DisplayDialog("Warning",
				"You're about to remove this state component.",
				"Continue", "Cancel"))
				Dispose();
		}
		serializedObject.ApplyModifiedProperties();
	}
}
