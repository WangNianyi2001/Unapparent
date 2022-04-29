using System;
using UnityEngine;

public class Billboard : MonoBehaviour {
	public Vector3 upward = new Vector3(0, 1, 0);
	public Vector3 orientation = new Vector3(0, 0, 1);
	[NonSerialized] public Vector3 forward, right;
	[NonSerialized] public Quaternion rotation;

	void Start() {
		forward = Vector3.ProjectOnPlane(orientation, upward).normalized;
		rotation = Quaternion.LookRotation(forward, upward);
		right = rotation * Vector3.right;
	}

	void Update() {
		transform.rotation = rotation;
	}
}
