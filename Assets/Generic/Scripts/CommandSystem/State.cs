using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class State : MonoBehaviour {
		[SerializeField] public List<Listener> listeners = new List<Listener>();

		[ContextMenu("Remove Component")]
		public void Dispose() {
			foreach(Listener listener in listeners)
				listener.Dispose();
			if(Application.isEditor) DestroyImmediate(this);
			else Destroy(this);
		}

		[ContextMenu("Reset")]
		public void Reset() {
			foreach(Listener listener in listeners)
				listener.Dispose();
			listeners.Clear();
		}

#if UNITY_EDITOR
		[MenuItem("GameObject/Unapparent/CreateState")]
		public static void CreateState() {
			GameObject state = new GameObject();
			state.name = "New State";
			state.AddComponent<State>();
			state.transform.parent = Selection.activeGameObject.transform;
		}
#endif
	}
}
