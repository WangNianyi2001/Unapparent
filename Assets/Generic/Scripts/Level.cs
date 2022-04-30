using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unapparent {
	public class Level : MonoBehaviour {
		public static Level current => SceneManager.GetActiveScene().GetRootGameObjects()[0]?.GetComponent<Level>();

		public new Camera camera;
		public Protagonist protagonist;
		[NonSerialized] public LevelUI ui;

		public Location startingLocation;
		Location location;

		void FireUnderLocation(Type type) {
			foreach(Carrier carrier in location.GetComponentsInChildren<Carrier>())
				carrier.AddToFireQueue(carrier.State, type);
		}

		public Location Location {
			get => location;
			set {
				FireUnderLocation(typeof(ExitLocation));
				location = value;
				FireUnderLocation(typeof(EnterLocation));
			}
		}

		void InitUI() {
			GameObject uiObj = Instantiate(
				Resources.Load<GameObject>("UI/Level UI"),
				transform
			);
			ui = uiObj.GetComponent<LevelUI>();
		}

		void Start() {
			InitUI();
		}
	}
}
