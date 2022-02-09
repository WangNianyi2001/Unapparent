using System;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	[Serializable]
	public class State : MonoBehaviour {
		[SerializeField] public Sequential sequence = new Sequential();
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			target.sequence.Inspect(IGUI.Nil, IGUI.Nil);
		}
	}
}
