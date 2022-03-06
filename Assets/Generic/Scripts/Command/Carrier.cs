using System;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public class Carrier : MonoBehaviour {
		public GameObject initialState = null;
		State currentState = null;

		public State state {
			get => currentState;
			set {
				currentState = value;
			}
		}

		public void TryFire(Type type) {
			foreach(Listener listener in currentState.listeners) {
				if(type.IsAssignableFrom(listener.GetType()))
					listener.TryExecute(this);
			}
		}

		public void Start() {
			state = initialState.GetComponent<State>();
			TryFire(typeof(OnStart));
		}
	}
}
