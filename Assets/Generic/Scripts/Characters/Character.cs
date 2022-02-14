using System;
using UnityEngine;

namespace Unapparent {
	public class Character : MonoBehaviour {
		public GameObject nodeObject = null;
		public Track.Node Node => nodeObject.GetComponentInParent<Track>().nodes.Find(
			(Track.Node node) => node.gameObject == nodeObject
		);

		Track.Segment segment;
		float distance = 0;
		
		void UpdatePosition() {
			transform.position = segment.GetPosition(distance);
		}

		public void MoveToNode(Track.Node node) {
			segment = node.next;
			distance = 0;
			UpdatePosition();
		}

		public void MoveBy(float distance) {
			distance += this.distance;
			while(distance < 0) {
				if(segment.from == null) {
					distance = 0;
					break;
				}
				distance += (segment = segment.Prev).Length;
			}
			while(distance > 0 && distance > segment.Length) {
				if(segment.to == null) {
					distance = 0;
					break;
				}
				distance -= segment.Length;
				segment = segment.Next;
			}
			if(segment.to == null)
				distance = 0;
			this.distance = distance;
			UpdatePosition();
		}

		public void OnValidate() {
			segment = Node.next;
			MoveToNode(Node);
		}
	}
}
