using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class Character : MonoBehaviour {
		public TrackContainer defaultTrack;
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

		#if UNITY_EDITOR
		private void OnValidate() {
			if (defaultTrack == null) return;
			track = defaultTrack.track;
			if (defaultDistance < 0) defaultDistance = 0;
			if (defaultDistance > track.distanceList.Last()) defaultDistance = track.distanceList.Last();
			MoveTo(defaultDistance);
		}

		[MenuItem("GameObject/角色")]
		public static void CreateCharacter() {
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			obj.transform.eulerAngles = new Vector3(90, 0, 180);
			obj.transform.localScale = new Vector3(.1f, 1, .1f);
			obj.AddComponent<Character>();
		}
		#endif
	}
}
