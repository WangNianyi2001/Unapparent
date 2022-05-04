using UnityEngine;

namespace Unapparent {
	public class Teleporter : MonoBehaviour {
		public Transform destination;
		TeleportTo command;

		void Start() {
			command = ScriptableObject.CreateInstance<TeleportTo>();
			command.destination = destination;
		}

		async void OnTriggerEnter(Collider other) {
			Character character = other.GetComponent<Character>();
			if(character == null)
				return;
			await command.Execute(character);
		}
	}
}