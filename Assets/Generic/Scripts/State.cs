using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class State : MonoBehaviour {
		public static List<Type> actionTypes = new List<Type> {
			//
		};

		public abstract class Action {
			public abstract void Execute();
			public abstract void Inspect();
		}

		public class ActionList : Sequential {
			public readonly string name;
			public ActionList(string name) {
				this.name = name;
			}
			public new void Inspect() {
				GUILayout.Label(name, EditorStyles.boldLabel);
				base.Inspect();
			}
		}

		public ActionList
			enter = new ActionList("Enter"),
			update = new ActionList("Update"),
			exit = new ActionList("Exit");
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			target.enter.Inspect();
			target.update.Inspect();
			target.exit.Inspect();
		}
	}
}