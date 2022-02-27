using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unapparent {
	public class State : MonoBehaviour {
		[Serializable]
		public struct Listener {
			public UnityEvent uEvent;
			[SerializeField] public Command command;
		}

		[SerializeField] public List<Listener> listeners = new List<Listener>();
	}
}
