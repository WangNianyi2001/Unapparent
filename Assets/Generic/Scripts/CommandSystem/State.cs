using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	public class State : MonoBehaviour {
		[SerializeField] public List<Listener> listeners = new List<Listener>();
	}
}
