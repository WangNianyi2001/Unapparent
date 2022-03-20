using System;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public class Carrier : MonoBehaviour {
		public State initialState = null;
		State state = null;
		public State State {
			get => state;
			set {
				TryFire(typeof(ExitState));
				state = value;
				TryFire(typeof(EnterState));
			}
		}

		public void TryFire(Type type) {
			if(state == null)
				return;
			foreach(Listener listener in state.listeners.elements) {
				if(type.IsAssignableFrom(listener.GetType()))
					listener.TryExecute(this);
			}
		}

		public void Start() {
			State = initialState;
		}
	}
}
