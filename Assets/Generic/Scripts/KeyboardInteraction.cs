using UnityEngine;

namespace Unapparent {
	public class KeyboardInteraction : MonoBehaviour {
		public Canvas ui;
		public KeyCode key;
		public Statement command;

		bool active = false;
		public bool Active {
			get => active;
			set {
				ui.gameObject.SetActive(value);
				active = value;
			}
		}

		void Start() {
			ui.gameObject.SetActive(false);
		}

		void Update() {
			if(Active && Input.GetKeyDown(key))
				command.Execute(Level.current.protagonist);
		}

		void OnTriggerEnter(Collider other) {
			if(other.gameObject != Level.current.protagonist.gameObject)
				return;
			Active = true;
		}

		void OnTriggerExit(Collider other) {
			if(other.gameObject != Level.current.protagonist.gameObject)
				return;
			Active = false;
		}
	}
}