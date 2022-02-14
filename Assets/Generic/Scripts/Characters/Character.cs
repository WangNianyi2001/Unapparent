using System.Linq;
using UnityEngine;

namespace Unapparent {
	public class Character : MonoBehaviour {
		public Track defaultTrack;
		public float defaultDistance;
		[HideInInspector]
		public Track track;
		public float Distance => distance;
		private float distance;
		private int nodeIndex;

		public void MoveTo(float distance) {
			this.distance = distance;
			transform.position = track.GetPosition(distance, nodeIndex);
		}

		public void Move(float delta) {
			MoveTo(distance + delta);
		}

		private void OnValidate() {
			if(defaultTrack == null)
				return;
			track = defaultTrack;
			if(defaultDistance < 0)
				defaultDistance = 0;
			if(defaultDistance > track.distanceList.Last())
				defaultDistance = track.distanceList.Last();
			MoveTo(defaultDistance);
		}
	}
}
