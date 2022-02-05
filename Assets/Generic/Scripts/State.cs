using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class State : MonoBehaviour {
		public static List<Type> actionTypes = new List<Type> {
			typeof(Sequence),
			typeof(SwitchState),
		};

		public Sequence sequence = new Sequence();
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			target.sequence.Inspect(Statement.Nil, Statement.Nil);
		}
	}
}