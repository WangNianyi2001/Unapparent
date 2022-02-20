using UnityEngine;
using UnityEditor;
using System;

namespace Unapparent {
	public class State : MonoBehaviour, IDisposable {
		[HideInInspector] public Command command = null;

		public void Reset() {
			Dispose();
			command = Command.Create(typeof(Sequential));
		}

		public void OnDestroy() {
			Dispose();
		}

		public void Dispose() {
			command?.Dispose();
		}
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
