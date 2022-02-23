﻿using UnityEngine;
using UnityEditor;

namespace Unapparent {
	[ExecuteInEditMode]
	public class State : MonoBehaviour {
		[SerializeField]
		public Command command = null;

#if UNITY_EDITOR
		public void Start() {
			command = Command.Create(typeof(Sequential));
		}
#endif

#if UNITY_EDITOR
		public void OnDestroy() {
			command?.Dispose();
		}
#endif
	}

	[CustomEditor(typeof(State))]
	public class StateEditor : Editor { }

	[CustomPropertyDrawer(typeof(Command))]
	public class CommandDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.PrefixLabel(position, label);
			Command target = property.objectReferenceValue as Command;
			target?.Inspect();
		}
	}
}
