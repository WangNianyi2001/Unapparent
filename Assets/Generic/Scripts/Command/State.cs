using UnityEngine;

namespace Unapparent {
	public class State : MonoBehaviour {
		public CommandList listeners;

		public void Reset() {
			listeners = new CommandList();
		}
	}
}
