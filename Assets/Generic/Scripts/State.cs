using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class State : MonoBehaviour {
		public Command sequence = typeof(Sequential);
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			target.sequence.Inspect(IGUI.Nil, IGUI.Nil);
		}
	}
}
