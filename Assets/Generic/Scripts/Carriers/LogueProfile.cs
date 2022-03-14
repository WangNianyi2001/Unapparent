using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class LogueProfile : ScriptableObject {
		public new string name;

		[MenuItem("Assets/Create/Unapparent/LogueProfile", false, 2)]
		public static void CreateLogueProfile() {
			LogueProfile so = CreateInstance<LogueProfile>();
			EditorGUIUtility.PingObject(so);
		}
	}
}
