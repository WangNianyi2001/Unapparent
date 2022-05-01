using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public static class EditorFunction {
		[MenuItem("Assets/Create/Unapparent/Identity", false, 1)]
		public static void CreateIdentityAsset() {
			Identity obj = ScriptableObject.CreateInstance<Identity>();
			AssetManager.CreateAssetInCurrentFolder(obj, "New Identity");
			EditorGUIUtility.PingObject(obj.GetInstanceID());
		}

		[MenuItem("GameObject/Unapparent/State", false, 1)]
		public static void CreateState() {
			GameObject gameObject = new GameObject("New State");
			gameObject.AddComponent<State>();
			if(Selection.activeGameObject == null)
				return;
			gameObject.transform.parent = Selection.activeGameObject.transform;
		}
	}
}
