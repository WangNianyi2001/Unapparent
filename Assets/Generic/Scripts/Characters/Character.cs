using UnityEngine;

namespace Unapparent {
	public class Character : MonoBehaviour {
		public GameObject nodeObject = null;
		public float distance = 0;
		Track.Node node;
		
		void UpdatePosition() {
			transform.position = node.next.GetPosition(distance);
		}

		public void MoveBy(float increment) {
			increment += distance;
			while(increment < 0) {
				if(node.From == null) {
					increment = 0;
					break;
				}
				increment += node.prev.Length;
				node = node.From;
			}
			while(increment >= node.next.Length) {
				if(node.Next == null) {
					increment = 0;
					break;
				}
				increment -= node.next.Length;
				node = node.Next;
			}
			distance = increment;
			UpdatePosition();
		}

		public void OnValidate() {
			node = nodeObject.GetComponentInParent<Track>().nodes.Find(
				(Track.Node node) => node.gameObject == nodeObject
			);
			UpdatePosition();
		}
	}
}
