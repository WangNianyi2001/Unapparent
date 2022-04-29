using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public class Carrier : MonoBehaviour {
		protected struct FireInfo {
			public State target;
			public Type type;
			public object[] arguments;
			public FireInfo(State target, Type type, params object[] arguments) {
				this.target = target;
				this.type = type;
				this.arguments = arguments;
			}
		}
		protected Queue<FireInfo> fires = new Queue<FireInfo>();

		public void AddToFireQueue(State target, Type type, params object[] arguments) {
			fires.Enqueue(new FireInfo(target, type, arguments));
		}

		public State initialState = null;
		Stack<State> currentStates = new Stack<State>();
		public State State {
			get => currentStates.Peek();
			set {
				State target = value;
				var buffer = new Stack<State>();
				while(currentStates.Count != 0 && !currentStates.Peek().isParentOf(target)) {
					var removed = currentStates.Pop();
					AddToFireQueue(removed, typeof(ExitState));
				}
				while(target != (currentStates.Count == 0 ? null : currentStates.Peek())) {
					buffer.Push(target);
					target = target.parent;
				}
				while(buffer.Count != 0) {
					var added = buffer.Pop();
					AddToFireQueue(added, typeof(EnterState));
					currentStates.Push(added);
				}
			}
		}

		public void ReceiveMessage(string message) {
			AddToFireQueue(State, typeof(ReceiveMessage), message);
		}

		[NonSerialized] public bool lastArrived = false;

		public virtual async Task<object> TeleportTo(Transform target) {
			await Task.Delay(1);
			transform.parent = target.parent;
			transform.position = target.position;
			return lastArrived = true;
		}

		public void Start() {
			State = initialState;
		}

		public void Update() {
			while(fires.Count != 0) {
				var info = fires.Dequeue();
				info.target.TryFire(info.type, this, info.arguments);
			}
		}

		public void OnTriggerEnter(Collider other) {
			AddToFireQueue(State, typeof(EnterTrigger), other);
		}

		public void OnTriggerExit(Collider other) {
			AddToFireQueue(State, typeof(ExitTrigger), other);
		}
	}
}
