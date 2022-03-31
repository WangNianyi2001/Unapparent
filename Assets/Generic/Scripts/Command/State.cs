using System;
using UnityEngine;

namespace Unapparent {
	public class State : MonoBehaviour {
		[SerializeReference] public State parent = null;

		public ListenerList listeners;

		public bool isParentOf(State target) {
			if(target == null)
				return false;
			return target == this || isParentOf(target.parent);
		}

		public void TryFire(Type type, Carrier target) {
			foreach(Listener listener in listeners.elements) {
				if(type.IsAssignableFrom(listener.GetType()))
					listener.TryExecute(target);
			}
		}

		public void Reset() {
			listeners = new ListenerList();
		}
	}
}
