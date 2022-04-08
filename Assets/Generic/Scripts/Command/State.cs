using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	public class State : MonoBehaviour {
		[SerializeReference] public State parent = null;

		public List<Listener> listeners;

		public bool isParentOf(State target) {
			if(target == null)
				return false;
			return target == this || isParentOf(target.parent);
		}

		public void TryFire(Type type, Carrier target, params object[] args) {
			foreach(Listener listener in listeners) {
				if(listener == null) {
					Debug.LogWarning($"Listener {target.name}/{name}/{type.Name} is null");
					continue;
				}
				if(type.IsAssignableFrom(listener.GetType()))
					listener.TryExecute(target, args);
			}
		}

		public void Reset() {
			listeners = new List<Listener>();
		}
	}
}
