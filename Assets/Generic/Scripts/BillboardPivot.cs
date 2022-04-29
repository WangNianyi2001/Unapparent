using UnityEngine;

public class BillboardPivot : MonoBehaviour {
	public enum Mode {
		LocalNegativeZ,
		Camera,
	};

	public Mode mode = Mode.LocalNegativeZ;

	public Camera targetCamera;

	void Update() {
		//
	}
}
