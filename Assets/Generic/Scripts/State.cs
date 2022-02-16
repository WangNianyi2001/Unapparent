using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class State : MonoBehaviour {
		[HideInInspector] public Command command = null;
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if(target.command == null)
				target.command = Command.Create(typeof(Sequential));
			target.command?.Inspect(null, null);
		}
	}
}
