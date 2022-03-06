using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class MonoCommand : MonoBehaviour {
		public Command command;

#if UNITY_EDITOR
		[MenuItem("GameObject/Unapparent/CreateMonoCommand")]
		public static void CreateMonoCommand() {
			GameObject mc = new GameObject();
			mc.name = "New MonoCommand";
			mc.AddComponent<MonoCommand>();
			mc.transform.parent = Selection.activeGameObject.transform;
		}
#endif
	}
}
