using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public class Carrier : MonoBehaviour {
		public State initialState = null;
		Stack<State> currentStates = new Stack<State>();
		public State State {
			get => currentStates.Peek();
			set {
				State target = value;
				var buffer = new Stack<State>();
				while(currentStates.Count != 0 && !currentStates.Peek().isParentOf(target)) {
					var removed = currentStates.Pop();
					removed.TryFire(typeof(ExitState), this);
				}
				while(target != (currentStates.Count == 0 ? null : currentStates.Peek())) {
					buffer.Push(target);
					target = target.parent;
				}
				while(buffer.Count != 0) {
					var added = buffer.Pop();
					added.TryFire(typeof(EnterState), this);
					currentStates.Push(added);
				}
			}
		}

		[NonSerialized] public bool lastArrived = false;

		public virtual async Task<object> TeleportTo(Vector3 position) {
			await Task.Delay(1);
			transform.position = position;
			return lastArrived = true;
		}

		public void OnTriggerEnter(Collider other) {
			State.TryFire(typeof(EnterTrigger), this, other);
		}

		public void OnTriggerExit(Collider other) {
			State.TryFire(typeof(ExitTrigger), this, other);
		}

		public void Start() {
			State = initialState;
		}
	}
}
