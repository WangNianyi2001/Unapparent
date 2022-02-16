using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class State : MonoBehaviour {
		public Command command = null;
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			target.command?.Inspect(null, null);
		}
	}
}
