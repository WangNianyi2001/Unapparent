using UnityEngine;

public class Billboard : MonoBehaviour {
	public Vector3 upRight = new Vector3(0, 1, 0);
	public Vector3 orientation = new Vector3(0, 0, 1);

	void Update() {
		Vector3 forward = Vector3.ProjectOnPlane(orientation, upRight).normalized;
		transform.rotation = Quaternion.LookRotation(forward, upRight);
	}
}
