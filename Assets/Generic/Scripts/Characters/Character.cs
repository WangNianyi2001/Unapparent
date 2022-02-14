using UnityEngine;

namespace Unapparent {
	public class Character : MonoBehaviour {
		Track track;
		float distance;
		int nodeIndex;

		public Track defaultTrack;
		public float defaultDistance;
		public float Distance => distance;

		private void OnValidate() {
			if(defaultTrack == null)
				return;
			track = defaultTrack;
			// Clamp position
			// Init position
		}
	}
}
