using UnityEngine;

namespace Unapparent {
	public class StateCarrier : MonoBehaviour {
		public GameObject initialState = null;
		protected State currentState = null;
		public void Start() {
			currentState = initialState.GetComponent<State>();
		}
	}
}