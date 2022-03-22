using UnityEngine;

namespace Unapparent {
	public class State : MonoBehaviour {
		[SerializeReference] public State parent = null;

		public ListenerList listeners;

		public void Reset() {
			listeners = new ListenerList();
		}
	}
}
