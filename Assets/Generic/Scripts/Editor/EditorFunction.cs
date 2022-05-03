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

		static GameObject CreateGameObjectWithSingleComponent<T>(string name = "Gameobject") where T : Component {
			GameObject gameObject = new GameObject(name);
			gameObject.AddComponent<T>();
			if(Selection.activeGameObject != null)
				gameObject.transform.parent = Selection.activeGameObject.transform;
			return gameObject;
		}

		[MenuItem("GameObject/Unapparent/State", false, 1)]
		public static void CreateState() {
			CreateGameObjectWithSingleComponent<State>("State");
		}

		[MenuItem("GameObject/Unapparent/Macro", false, 1)]
		public static void CreateMacro() {
			CreateGameObjectWithSingleComponent<Macro>("Macro");
		}
	}
}
