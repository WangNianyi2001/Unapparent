using UnityEngine;

namespace Unapparent {
	public class Character : MonoBehaviour {
		public GameObject initNodeObject = null;
		public Track.Node InitNode => initNodeObject.GetComponentInParent<Track>().nodes.Find(
			(Track.Node node) => node.gameObject == initNodeObject
		);

		public void OnValidate() {
			transform.position = InitNode.Pos;
		}
	}
}
