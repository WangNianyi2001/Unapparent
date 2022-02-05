using UnityEngine;

namespace Unapparent {
	public class Carrier : MonoBehaviour {
		public GameObject initialState = null;
		protected State currentState = null;
		public void Start() {
			currentState = initialState.GetComponent<State>();
		}
	}
}