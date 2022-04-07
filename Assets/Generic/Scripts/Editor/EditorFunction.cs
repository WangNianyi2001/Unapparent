using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public static class EditorFunction {
		[MenuItem("Assets/Create/Unapparent/Identity", false, 1)]
		public static void CreateIdentityAsset() {
			Identity obj = ScriptableObject.CreateInstance<Identity>();
			ManagedAsset.CreateAsset(obj, "Identity", "New Identity");
			EditorGUIUtility.PingObject(obj.GetInstanceID());
		}
	}
}
