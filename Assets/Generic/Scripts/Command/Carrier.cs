using System;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public class Carrier : MonoBehaviour {
		public GameObject initialState = null;
		State currentState = null;

		public void Start() {
			currentState = initialState.GetComponent<State>();
		}
	}
}
