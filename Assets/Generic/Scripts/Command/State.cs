using UnityEngine;

namespace Unapparent {
	public class State : MonoBehaviour {
		public ListenerList listeners;

		public void Reset() {
			listeners = new ListenerList();
		}
	}
}
